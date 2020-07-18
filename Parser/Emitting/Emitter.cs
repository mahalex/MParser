using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Parser.Binding;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Parser.Emitting
{
    public class MethodInterfaceDescription
    {
        public MethodInterfaceDescription(ImmutableArray<ParameterSymbol> inputDescription, ImmutableArray<ParameterSymbol> outputDescription, MethodDefinition method)
        {
            InputDescription = inputDescription;
            OutputDescription = outputDescription;
            Method = method;
        }

        public ImmutableArray<ParameterSymbol> InputDescription { get; }

        public ImmutableArray<ParameterSymbol> OutputDescription { get; }

        public MethodDefinition Method { get; }
    }

    public class Emitter
    {
        private Dictionary<string, TypeReference> _knownTypes = new Dictionary<string, TypeReference>();
        private Dictionary<string, MethodInterfaceDescription> _functions = new Dictionary<string, MethodInterfaceDescription>();
        private MethodReference? _consoleWriteLineReference;
        private MethodReference? _dispReference;
        private MethodReference? _stringToMObject;
        private MethodReference? _doubleToMObject;
        private MethodReference? _getItemFromDictionary;
        private MethodReference? _putItemIntoDictionary;
        private TypeReference? _mObjectType;
        private ArrayType? _mObjectArrayType;
        private Dictionary<string, VariableDefinition> _currentOutputVariables = new Dictionary<string, VariableDefinition>();
        private VariableDefinition? _currentLocals = null;
        private TypeSpecification? _stringMObjectDictionary = null;
        private MethodReference? _dictionaryCtorReference = null;
        private Dictionary<BoundBinaryOperatorKind, MethodReference> _binaryOperations = new Dictionary<BoundBinaryOperatorKind, MethodReference>();
        private Dictionary<BoundUnaryOperatorKind, MethodReference> _unaryOperations = new Dictionary<BoundUnaryOperatorKind, MethodReference>();

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
                "System.Void",
                "System.String",
                "System.Collections.Generic.Dictionary`2",
                "Parser.Objects.MObject"
            };

            // Resolve built-in types and methods.
            foreach (var typeName in builtInTypes)
            {
                var typeReference = ResolveAndImportType(typeName, assemblies, assemblyDefinition);
                _knownTypes.Add(typeName, typeReference);
            }

            var objectType = _knownTypes["System.Object"];
            var voidType = _knownTypes["System.Void"];
            var stringType = _knownTypes["System.String"];
            var dictionaryType = _knownTypes["System.Collections.Generic.Dictionary`2"];
            _mObjectType = _knownTypes["Parser.Objects.MObject"];
            _mObjectArrayType = _mObjectType.MakeArrayType();
            var stringMObjectDictionary = new GenericInstanceType(dictionaryType);
            stringMObjectDictionary.GenericArguments.Add(stringType);
            stringMObjectDictionary.GenericArguments.Add(_mObjectType);
            _stringMObjectDictionary = stringMObjectDictionary;
            _dictionaryCtorReference = ResolveAndImportMethod(
                typeName: "System.Collections.Generic.Dictionary`2",
                methodName: ".ctor",
                parameterTypeNames: Array.Empty<string>(),
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);
            _dictionaryCtorReference.DeclaringType = _stringMObjectDictionary;
            _getItemFromDictionary = ResolveAndImportMethod(
                typeName: "System.Collections.Generic.Dictionary`2",
                methodName: "get_Item",
                parameterTypeNames: new[] { "TKey" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);
            _getItemFromDictionary.DeclaringType = _stringMObjectDictionary;
            _putItemIntoDictionary = ResolveAndImportMethod(
                typeName: "System.Collections.Generic.Dictionary`2",
                methodName: "set_Item",
                parameterTypeNames: new[] { "TKey", "TValue" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);
            _putItemIntoDictionary.DeclaringType = _stringMObjectDictionary;

            _consoleWriteLineReference = ResolveAndImportMethod(
                typeName: "System.Console",
                methodName: "WriteLine",
                parameterTypeNames: new[] { "System.Object" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            _dispReference = ResolveAndImportMethod(
                typeName: "Parser.MFunctions.MHelpers",
                methodName: "Disp",
                parameterTypeNames: new[] { "Parser.Objects.MObject" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            _stringToMObject = ResolveAndImportMethod(
                typeName: "Parser.Objects.MObject",
                methodName: "CreateCharArray",
                parameterTypeNames: new[] { "System.String" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            _doubleToMObject = ResolveAndImportMethod(
                typeName: "Parser.Objects.MObject",
                methodName: "CreateDoubleNumber",
                parameterTypeNames: new[] { "System.Double" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            var binaryOperationNames = new Dictionary<BoundBinaryOperatorKind, string>
            {
                [BoundBinaryOperatorKind.Plus] = "Plus",
                [BoundBinaryOperatorKind.Minus] = "Minus",
                [BoundBinaryOperatorKind.Star] = "Star",
                [BoundBinaryOperatorKind.Slash] = "Slash",
                [BoundBinaryOperatorKind.Greater] = "Greater",
                [BoundBinaryOperatorKind.GreaterOrEquals] = "GreaterOrEquals",
                [BoundBinaryOperatorKind.Less] = "Less",
                [BoundBinaryOperatorKind.LessOrEquals] = "LessOrEquals",
            };

            var unaryOperationNames = new Dictionary<BoundUnaryOperatorKind, string>
            {
                [BoundUnaryOperatorKind.Minus] = "Minus",
            };

            foreach (var (op, opName) in binaryOperationNames)
            {
                _binaryOperations[op] = ResolveAndImportMethod(
                    typeName: "Parser.MFunctions.MOperations",
                    methodName: opName,
                    parameterTypeNames: new[] { "Parser.Objects.MObject", "Parser.Objects.MObject" },
                    assemblies: assemblies,
                    assemblyDefinition: assemblyDefinition);
            }

            foreach (var (op, opName) in unaryOperationNames)
            {
                _unaryOperations[op] = ResolveAndImportMethod(
                    typeName: "Parser.MFunctions.MOperations",
                    methodName: opName,
                    parameterTypeNames: new[] { "Parser.Objects.MObject" },
                    assemblies: assemblies,
                    assemblyDefinition: assemblyDefinition);
            }

            // Create type.
            var typeDefinition = new TypeDefinition(
                @namespace: "",
                name: "Program",
                attributes: TypeAttributes.Abstract | TypeAttributes.Sealed,
                baseType: objectType);
            assemblyDefinition.MainModule.Types.Add(typeDefinition);

            // Generate method definitions for all functions first.
            foreach (var (name, body) in program.Functions)
            {
                var methodDefinition = new MethodDefinition(
                    name: name.Name,
                    attributes: MethodAttributes.Static | MethodAttributes.Private,
                    returnType: body.OutputDescription.Length == 0 ? voidType : _mObjectArrayType);
                if (body.InputDescription.Length > 0)
                {
                    foreach (var inputDescription in body.InputDescription)
                    {
                        var parameter = new ParameterDefinition(
                            name: inputDescription.Name,
                            attributes: ParameterAttributes.None,
                            parameterType: _mObjectType);
                        methodDefinition.Parameters.Add(parameter);
                    }
                }

                _functions[name.Name] = new MethodInterfaceDescription(
                    inputDescription: body.InputDescription,
                    outputDescription: body.OutputDescription,
                    method: methodDefinition);
            }

            // Emit functions.
            foreach (var (name, body) in program.Functions)
            {
                var methodDefinition = _functions[name.Name];
                EmitFunction(body, methodDefinition.Method);
                typeDefinition.Methods.Add(methodDefinition.Method);
            }

            // Set entry point.
            if (program.ScriptFunction is { } scriptFunction)
            {
                assemblyDefinition.EntryPoint = _functions[scriptFunction.Name].Method;
            }
            else if (program.MainFunction is { } mainFunction)
            {
                assemblyDefinition.EntryPoint = _functions[mainFunction.Name].Method;
            }

            assemblyDefinition.Write(outputFileName);
        }

        private void EmitFunction(LoweredFunction function, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();

            // Local #0 is the dictionary with actual local variables.
            _currentLocals = new VariableDefinition(_stringMObjectDictionary);
            ilProcessor.Body.Variables.Add(_currentLocals);
            ilProcessor.Emit(OpCodes.Newobj, _dictionaryCtorReference);
            ilProcessor.Emit(OpCodes.Stloc_0);
            var counter = 0;
            foreach (var inputDescription in function.InputDescription)
            {
                var name = inputDescription.Name;
                ilProcessor.Emit(OpCodes.Ldloc_0);
                ilProcessor.Emit(OpCodes.Ldstr, name);
                ilProcessor.Emit(OpCodes.Ldarg, methodDefinition.Parameters[counter]);
                ilProcessor.Emit(OpCodes.Callvirt, _putItemIntoDictionary);
                counter++;
            }

            // The following locals are "output variables".
            _currentOutputVariables.Clear();
            if (function.OutputDescription.Length > 0)
            {
                foreach (var outputDescription in function.OutputDescription)
                {
                    var outputVariable = new VariableDefinition(_mObjectArrayType);
                    ilProcessor.Body.Variables.Add(outputVariable);
                    _currentOutputVariables.Add(outputDescription.Name, outputVariable);
                }
            }

            EmitBlockStatement(function.Body, methodDefinition);

            // Copy output variables to the output array.
            if (function.OutputDescription.Length > 0)
            {
                ilProcessor.Emit(OpCodes.Ldc_I4, function.OutputDescription.Length);
                ilProcessor.Emit(OpCodes.Newarr, _mObjectType);
                for (var i = 0; i < function.OutputDescription.Length; i++)
                {
                    ilProcessor.Emit(OpCodes.Dup);
                    ilProcessor.Emit(OpCodes.Ldc_I4, i);
                    ilProcessor.Emit(OpCodes.Ldloc, i + 1);
                    ilProcessor.Emit(OpCodes.Stelem_Ref);
                }
            }

            ilProcessor.Emit(OpCodes.Ret);
        }

        private void EmitBlockStatement(BoundBlockStatement block, MethodDefinition methodDefinition)
        {
            var index = 0;
            while (index < block.Statements.Length)
            {
                var statement = block.Statements[index];
                switch (statement.Kind)
                {
                    case BoundNodeKind.GotoStatement:
                        throw new NotImplementedException("Gotos are not supported.");
                    case BoundNodeKind.ConditionalGotoStatement:
                        throw new NotImplementedException("Conditional gotos are not supported.");
                    case BoundNodeKind.LabelStatement:
                        throw new NotImplementedException("Labels are not supported.");
                    default:
                        EmitStatement(statement, methodDefinition);
                        index++;
                        break;
                }
            }

        }

        private void EmitStatement(BoundStatement node, MethodDefinition methodDefinition)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.EmptyStatement:
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EmitExpressionStatement((BoundExpressionStatement)node, methodDefinition);
                    break;
                default:
                    throw new NotImplementedException($"Invalid statement kind '{node.Kind}'.");
            };
        }

        private void EmitExpressionStatement(BoundExpressionStatement node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            EmitExpression(node.Expression, methodDefinition);
            if (node.DiscardResult)
            {
                ilProcessor.Emit(OpCodes.Pop);
            }
            else
            {
                ilProcessor.Emit(OpCodes.Call, _dispReference);
            }
        }

        private void EmitExpression(BoundExpression node, MethodDefinition methodDefinition)
        {
            switch (node.Kind) {
                case BoundNodeKind.AssignmentExpression:
                    EmitAssignmentExpression((BoundAssignmentExpression)node, methodDefinition);
                    break;
                case BoundNodeKind.BinaryOperationExpression:
                    EmitBinaryOperationExpression((BoundBinaryOperationExpression)node, methodDefinition);
                    break;
                case BoundNodeKind.FunctionCallExpression:
                    EmitFunctionCallExpression((BoundFunctionCallExpression)node, methodDefinition);
                    break;
                case BoundNodeKind.IdentifierNameExpression:
                    EmitIdentifierNameExpression((BoundIdentifierNameExpression)node, methodDefinition);
                    break;
                case BoundNodeKind.NumberLiteralExpression:
                    EmitNumberLiteralExpression((BoundNumberLiteralExpression)node, methodDefinition);
                    break;
                case BoundNodeKind.StringLiteralExpression:
                    EmitStringLiteralExpression((BoundStringLiteralExpression)node, methodDefinition);
                    break;
                default:
                    throw new NotImplementedException($"Invalid node kind '{node.Kind}'.");
            }
        }

        private void EmitBinaryOperationExpression(BoundBinaryOperationExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            var method = _binaryOperations[node.Op.Kind];
            EmitExpression(node.Left, methodDefinition);
            EmitExpression(node.Right, methodDefinition);
            ilProcessor.Emit(OpCodes.Call, method);
        }

        private void EmitAssignmentExpression(BoundAssignmentExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            if (node.Left.Kind != BoundNodeKind.IdentifierNameExpression) 
            {
                throw new Exception("Assignment to complex lvalues is not supported.");
            }
            var left = ((BoundIdentifierNameExpression)node.Left);
            ilProcessor.Emit(OpCodes.Ldloc_0);
            ilProcessor.Emit(OpCodes.Ldstr, left.Name);
            EmitExpression(node.Right, methodDefinition);
            ilProcessor.Emit(OpCodes.Callvirt, _putItemIntoDictionary);
            ilProcessor.Emit(OpCodes.Ldloc_0);
            ilProcessor.Emit(OpCodes.Ldstr, left.Name);
            ilProcessor.Emit(OpCodes.Callvirt, _getItemFromDictionary);
        }

        private void EmitIdentifierNameExpression(BoundIdentifierNameExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            ilProcessor.Emit(OpCodes.Ldloc_0);
            ilProcessor.Emit(OpCodes.Ldstr, node.Name);
            ilProcessor.Emit(OpCodes.Callvirt, _getItemFromDictionary);
        }

        private void EmitNumberLiteralExpression(BoundNumberLiteralExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            ilProcessor.Emit(OpCodes.Ldc_R8, node.Value);
            ilProcessor.Emit(OpCodes.Call, _doubleToMObject);
        }

        private void EmitStringLiteralExpression(BoundStringLiteralExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            ilProcessor.Emit(OpCodes.Ldstr, node.Value);
            ilProcessor.Emit(OpCodes.Call, _stringToMObject);
        }

        private void EmitFunctionCallExpression(BoundFunctionCallExpression node, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            if (node.Name.Kind == BoundNodeKind.IdentifierNameExpression
                && ((BoundIdentifierNameExpression)node.Name).Name == "disp")
            {
                EmitExpression(node.Arguments[0], methodDefinition);
                ilProcessor.Emit(OpCodes.Call, _dispReference);
                ilProcessor.Emit(OpCodes.Ldnull);
            }
            else
            {
                var function = ResolveFunction(node.Name);
                for (var i = 0; i < function.InputDescription.Length; i++)
                {
                    if (i < node.Arguments.Length)
                    {
                        EmitExpression(node.Arguments[i], methodDefinition);
                    }
                    else
                    {
                        ilProcessor.Emit(OpCodes.Ldnull);
                    }
                }

                ilProcessor.Emit(OpCodes.Call, function.Method);
                if (function.OutputDescription.Length == 0)
                {
                    ilProcessor.Emit(OpCodes.Ldnull);
                }
                else if (function.OutputDescription.Length == 1)
                {
                    ilProcessor.Emit(OpCodes.Ldc_I4_0);
                    ilProcessor.Emit(OpCodes.Ldelem_Ref);
                }
                else
                {
                    throw new NotImplementedException("Functions with multiple output are not supported.");
                }
            }
        }

        private MethodInterfaceDescription ResolveFunction(BoundExpression expression)
        {
            if (expression.Kind == BoundNodeKind.IdentifierNameExpression)
            {
                var name = ((BoundIdentifierNameExpression)expression).Name;
                if (_functions.TryGetValue(name, out var result))
                {
                    return result;
                } else
                {
                    throw new Exception($"Function '{name}' not found.");
                }
            }
            else
            {
                throw new NotImplementedException($"Dynamic functions calling not supported. Failed to resolve function call expression with kind '{expression.Kind}'.");
            }
        }
    }
}
