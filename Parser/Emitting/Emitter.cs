using Mono.Cecil;
using Mono.Cecil.Cil;
using Parser.Binding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parser.Emitting
{
    public class Emitter
    {
        private Dictionary<string, TypeReference> _knownTypes = new Dictionary<string, TypeReference>();

        private static TypeReference ResolveAndImportType(
            string typeName,
            List<AssemblyDefinition> assemblies,
            AssemblyDefinition assemblyDefinition)
        {
            var foundTypes = assemblies.SelectMany(a => a.Modules)
                                       .SelectMany(m => m.Types)
                                       .Where(t => t.FullName == typeName)
                                       .ToArray();
            if (foundTypes.Length == 1)
            {
                var typeReference = assemblyDefinition.MainModule.ImportReference(foundTypes[0]);
                return typeReference;
            }
            else if (foundTypes.Length == 0)
            {
                throw new Exception($"Cannot find type {typeName}");
            }
            else
            {
                throw new Exception($"Ambiguous type {typeName}");
            }
        }

        private static MethodReference ResolveAndImportMethod(
            string typeName,
            string methodName,
            string[] parameterTypeNames,
            List<AssemblyDefinition> assemblies,
            AssemblyDefinition assemblyDefinition)
        {
            var foundTypes = assemblies.SelectMany(a => a.Modules)
                                       .SelectMany(m => m.Types)
                                       .Where(t => t.FullName == typeName)
                                       .ToArray();
            if (foundTypes.Length == 1)
            {
                var foundType = foundTypes[0];
                var methods = foundType.Methods.Where(m => m.Name == methodName);

                foreach (var method in methods)
                {
                    if (method.Parameters.Count != parameterTypeNames.Length)
                        continue;

                    var allParametersMatch = true;

                    for (var i = 0; i < parameterTypeNames.Length; i++)
                    {
                        if (method.Parameters[i].ParameterType.FullName != parameterTypeNames[i])
                        {
                            allParametersMatch = false;
                            break;
                        }
                    }

                    if (!allParametersMatch)
                        continue;

                    return assemblyDefinition.MainModule.ImportReference(method);
                }

                throw new Exception($"Required method {typeName}.{methodName} not found.");
            }
            else if (foundTypes.Length == 0)
            {
                throw new Exception($"Required type {typeName} not found.");
            }
            else
            {
                throw new Exception($"Required type {typeName} is ambiguous.");
            }
        }

        public void Emit(BoundProgram program, string[] references, string outputFileName)
        {
            var assemblies = new List<AssemblyDefinition>();

            foreach (var reference in references)
            {
                try
                {
                    var assembly = AssemblyDefinition.ReadAssembly(reference);
                    assemblies.Add(assembly);
                }
                catch (BadImageFormatException)
                {
                    throw new Exception($"Invalid reference '{reference}'.");
                }
            }

            var moduleName = Path.GetFileNameWithoutExtension(outputFileName);
            var assemblyName = new AssemblyNameDefinition(
                name: moduleName,
                version: new Version(1, 0));
            var assemblyDefinition = AssemblyDefinition.CreateAssembly(
                assemblyName: assemblyName,
                moduleName: moduleName,
                kind: ModuleKind.Console);
            var builtInTypes = new[]
            {
                "System.Object",
                "System.Void"
            };

            // Resolve built-in types and methods.
            foreach (var typeName in builtInTypes)
            {
                var typeReference = ResolveAndImportType(typeName, assemblies, assemblyDefinition);
                _knownTypes.Add(typeName, typeReference);
            }

            var objectType = _knownTypes["System.Object"];
            var voidType = _knownTypes["System.Void"];

            var consoleWriteLineReference = ResolveAndImportMethod(
                typeName: "System.Console",
                methodName: "WriteLine",
                parameterTypeNames: new[] { "System.Object" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            // Create type.
            var typeDefinition = new TypeDefinition(
                @namespace: "",
                name: "Program",
                attributes: TypeAttributes.Abstract | TypeAttributes.Sealed,
                baseType: objectType);
            assemblyDefinition.MainModule.Types.Add(typeDefinition);

            // Create method.
            var methodDefinition = new MethodDefinition(
                name: "Main",
                attributes: MethodAttributes.Static | MethodAttributes.Private,
                returnType: voidType);
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            ilProcessor.Emit(OpCodes.Ldstr, "Hello world!");
            ilProcessor.Emit(OpCodes.Call, consoleWriteLineReference);
            ilProcessor.Emit(OpCodes.Ret);

            typeDefinition.Methods.Add(methodDefinition);
            assemblyDefinition.EntryPoint = methodDefinition;

            assemblyDefinition.Write(outputFileName);
        }
    }
}
