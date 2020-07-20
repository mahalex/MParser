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
    public static class BuiltInFunctions
    {
        public static readonly TypedFunctionSymbol Disp = new TypedFunctionSymbol(
            "disp",
            ImmutableArray.Create(
                new TypedParameterSymbol("text", TypeSymbol.MObject)),
            TypeSymbol.Void);
    }

    public class Emitter
    {
        private Dictionary<string, TypeReference> _knownTypes = new Dictionary<string, TypeReference>();
        private Dictionary<TypeSymbol, TypeReference> _resolvedTypes = new Dictionary<TypeSymbol, TypeReference>();
        private Dictionary<string, MethodInterfaceDescription> _functions = new Dictionary<string, MethodInterfaceDescription>();
        private MethodReference? _consoleWriteLineReference;
        private MethodReference? _dispReference;
        private MethodReference? _lenReference;
        private MethodReference? _arraySliceReference;
        private MethodReference? _mObjectToBool;
        private MethodReference? _stringToMObject;
        private MethodReference? _doubleToMObject;
        private MethodReference? _intToMObject;
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
        private Dictionary<BoundLabel, int> _labels = new Dictionary<BoundLabel, int>();
        private Dictionary<int, BoundLabel> _forwardLabelsToFix = new Dictionary<int, BoundLabel>();
        private Dictionary<TypedVariableSymbol, VariableDefinition> _typedLocals = new Dictionary<TypedVariableSymbol, VariableDefinition>();
        private Dictionary<TypedFunctionSymbol, MethodReference> _builtInFunctions = new Dictionary<TypedFunctionSymbol, MethodReference>();

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
                "System.Int32",
                "System.Object",
                "System.Void",
                "System.String",
                "System.Collections.Generic.Dictionary`2",
                "Parser.Objects.MObject"
            };

            var typeSymbolToKnownType = new Dictionary<TypeSymbol, string>
            {
                [TypeSymbol.Int] = "System.Int32",
                [TypeSymbol.MObject] = "Parser.Objects.MObject",
            };

            // Resolve built-in types and methods.
            foreach (var typeName in builtInTypes)
            {
                var typeReference = ResolveAndImportType(typeName, assemblies, assemblyDefinition);
                _knownTypes.Add(typeName, typeReference);
            }

            foreach (var (typeSymbol, knownTypeName) in typeSymbolToKnownType)
            {
                _resolvedTypes.Add(typeSymbol, _knownTypes[knownTypeName]);
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

            _lenReference = ResolveAndImportMethod(
                typeName: "Parser.MFunctions.MHelpers",
                methodName: "Len",
                parameterTypeNames: new[] { "Parser.Objects.MObject" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            _arraySliceReference = ResolveAndImportMethod(
                typeName: "Parser.MFunctions.MOperations",
                methodName: "ArraySlice",
                parameterTypeNames: new[] { "Parser.Objects.MObject", "Parser.Objects.MObject" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            _mObjectToBool = ResolveAndImportMethod(
                typeName: "Parser.MFunctions.MHelpers",
                methodName: "ToBool",
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

            _intToMObject = ResolveAndImportMethod(
                typeName: "Parser.Objects.MObject",
                methodName: "CreateIntNumber",
                parameterTypeNames: new[] { "System.Int32" },
                assemblies: assemblies,
                assemblyDefinition: assemblyDefinition);

            var binaryOperationNames = new Dictionary<BoundBinaryOperatorKind, string>
            {
                [BoundBinaryOperatorKind.Colon] = "Colon",
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

            _builtInFunctions.Add(
                BuiltInFunctions.Disp,
                _dispReference);

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
            Console.WriteLine($"Emitting function '{function.Name}'.");
            var ilProcessor = methodDefinition.Body.GetILProcessor();

            _labels.Clear();
            _forwardLabelsToFix.Clear();
            _typedLocals.Clear();

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

            foreach (var (index, target) in _forwardLabelsToFix)
            {
                var targetIndex = _labels[target];
                var left = ilProcessor.Body.Instructions[index];
                var right = ilProcessor.Body.Instructions[targetIndex];
                left.Operand = right;
            }

        }

        private void EmitBlockStatement(BoundBlockStatement block, MethodDefinition methodDefinition)
        {
            var ilProcessor = methodDefinition.Body.GetILProcessor();
            foreach (var statement in block.Statements)
            {
                switch (statement.Kind)
                {
                    case BoundNodeKind.GotoStatement:
                        EmitGoto((BoundGotoStatement)statement, ilProcessor);
                        break;
                    case BoundNodeKind.ConditionalGotoStatement:
                        EmitConditionalGoto((BoundConditionalGotoStatement)statement, ilProcessor);
                        break;
                    case BoundNodeKind.LabelStatement:
                        EmitLabelStatement((BoundLabelStatement)statement, ilProcessor);
                        break;
                    default:
                        EmitStatement(statement, ilProcessor);
                        break;
                }
            }
        }

        private void EmitLabelStatement(BoundLabelStatement node, ILProcessor ilProcessor)
        {
            _labels[node.Label] = ilProcessor.Body.Instructions.Count;
        }

        private void EmitGoto(BoundGotoStatement node, ILProcessor ilProcessor)
        {
            if (_labels.TryGetValue(node.Label, out var target))
            {
                ilProcessor.Emit(OpCodes.Br, ilProcessor.Body.Instructions[target]);
            }
            else
            {
                _forwardLabelsToFix.Add(ilProcessor.Body.Instructions.Count, node.Label);
                ilProcessor.Emit(OpCodes.Br, Instruction.Create(OpCodes.Nop));
            }
        }

        private void EmitConditionalGoto(BoundConditionalGotoStatement node, ILProcessor ilProcessor)
        {
            EmitExpression(node.Condition, ilProcessor);
            ilProcessor.Emit(OpCodes.Call, _mObjectToBool);
            var instruction = node.GotoIfTrue ? OpCodes.Brtrue : OpCodes.Brfalse;
            if (_labels.TryGetValue(node.Label, out var target))
            {
                ilProcessor.Emit(instruction, ilProcessor.Body.Instructions[target]);
            }
            else
            {
                _forwardLabelsToFix.Add(ilProcessor.Body.Instructions.Count, node.Label);
                ilProcessor.Emit(instruction, Instruction.Create(OpCodes.Nop));
            }
        }

        private void EmitStatement(BoundStatement node, ILProcessor ilProcessor)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.EmptyStatement:
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EmitExpressionStatement((BoundExpressionStatement)node, ilProcessor);
                    break;
                case BoundNodeKind.TypedVariableDeclaration:
                    EmitTypedVariableDeclaration((BoundTypedVariableDeclaration)node, ilProcessor);
                    break;
                default:
                    throw new NotImplementedException($"Invalid statement kind '{node.Kind}'.");
            };
        }

        private void EmitTypedVariableDeclaration(BoundTypedVariableDeclaration node, ILProcessor ilProcessor)
        {
            var typeReference = _resolvedTypes[node.Variable.Type];
            var variableDefinition = new VariableDefinition(typeReference);
            _typedLocals.Add(node.Variable, variableDefinition);
            ilProcessor.Body.Variables.Add(variableDefinition);

            EmitExpression(node.Initializer, ilProcessor);
            ilProcessor.Emit(OpCodes.Stloc, variableDefinition);
        }

        private void EmitExpressionStatement(BoundExpressionStatement node, ILProcessor ilProcessor)
        {
            EmitExpression(node.Expression, ilProcessor);
            if (node.DiscardResult)
            {
                ilProcessor.Emit(OpCodes.Pop);
            }
            else
            {
                ilProcessor.Emit(OpCodes.Call, _dispReference);
            }
        }

        private void EmitExpression(BoundExpression node, ILProcessor ilProcessor)
        {
            switch (node.Kind) {
                case BoundNodeKind.AssignmentExpression:
                    EmitAssignmentExpression((BoundAssignmentExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.BinaryOperationExpression:
                    EmitBinaryOperationExpression((BoundBinaryOperationExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.ConversionExpression:
                    EmitConversionExpression((BoundConversionExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.FunctionCallExpression:
                    EmitFunctionCallExpression((BoundFunctionCallExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.IdentifierNameExpression:
                    EmitIdentifierNameExpression((BoundIdentifierNameExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.NumberLiteralExpression:
                    EmitNumberLiteralExpression((BoundNumberDoubleLiteralExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.StringLiteralExpression:
                    EmitStringLiteralExpression((BoundStringLiteralExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.TypedFunctionCallExpression:
                    EmitTypedFunctionCallExpression((BoundTypedFunctionCallExpression)node, ilProcessor);
                    break;
                case BoundNodeKind.TypedVariableExpression:
                    EmitTypedVariableExpression((BoundTypedVariableExpression)node, ilProcessor);
                    break;
                default:
                    throw new NotImplementedException($"Invalid node kind '{node.Kind}'.");
            }
        }

        private void EmitConversionExpression(BoundConversionExpression node, ILProcessor ilProcessor)
        {
            var fromType = node.Expression.Type;
            var toType = node.TargetType;
            EmitExpression(node.Expression, ilProcessor);
            if ((fromType, toType) == (TypeSymbol.Double, TypeSymbol.MObject))
            {
                ilProcessor.Emit(OpCodes.Call, _doubleToMObject);
            }
            else if ((fromType, toType) == (TypeSymbol.String, TypeSymbol.MObject))
            {
                ilProcessor.Emit(OpCodes.Call, _stringToMObject);
            }
            else
            {
                throw new NotImplementedException($"Conversion of '{fromType}' to '{toType}' is not implemented.");
            }
        }

        private void EmitTypedVariableExpression(BoundTypedVariableExpression node, ILProcessor ilProcessor)
        {
            var variableDefinition = _typedLocals[node.Variable];
            ilProcessor.Emit(OpCodes.Ldloc, variableDefinition);
        }

        private void EmitBinaryOperationExpression(BoundBinaryOperationExpression node, ILProcessor ilProcessor)
        {
            var method = _binaryOperations[node.Op.Kind];
            EmitExpression(node.Left, ilProcessor);
            EmitExpression(node.Right, ilProcessor);
            ilProcessor.Emit(OpCodes.Call, method);
        }

        private void EmitAssignmentExpression(BoundAssignmentExpression node, ILProcessor ilProcessor)
        {
            var leftType = node.Left.Kind switch
            {
                BoundNodeKind.IdentifierNameExpression => TypeSymbol.MObject,
                BoundNodeKind.TypedVariableExpression => ((BoundTypedVariableExpression)node.Left).Type,
                _ => throw new Exception($"Assignment to lvalue of kind {node.Left.Kind} is not supported."),
            };

            var rightType = node.Right.Type;

            var rewrittenRight = ConvertExpression(node.Right, leftType);

            if (node.Left.Kind == BoundNodeKind.IdentifierNameExpression)
            {
                var left = ((BoundIdentifierNameExpression)node.Left);
                ilProcessor.Emit(OpCodes.Ldloc_0);
                ilProcessor.Emit(OpCodes.Ldstr, left.Name);
                EmitExpression(rewrittenRight, ilProcessor);
                ilProcessor.Emit(OpCodes.Callvirt, _putItemIntoDictionary);
                ilProcessor.Emit(OpCodes.Ldloc_0);
                ilProcessor.Emit(OpCodes.Ldstr, left.Name);
                ilProcessor.Emit(OpCodes.Callvirt, _getItemFromDictionary);
            }
            else if (node.Left.Kind == BoundNodeKind.TypedVariableExpression)
            {
                var typedVariableExpression = (BoundTypedVariableExpression)node.Left;
                EmitExpression(rewrittenRight, ilProcessor);
                ilProcessor.Emit(OpCodes.Stloc, _typedLocals[typedVariableExpression.Variable]);
            }
            else
            {
                throw new Exception($"Assignment to lvalue of kind {node.Left.Kind} is not supported.");
            }
        }

        private void EmitIdentifierNameExpression(BoundIdentifierNameExpression node, ILProcessor ilProcessor)
        {
            ilProcessor.Emit(OpCodes.Ldloc_0);
            ilProcessor.Emit(OpCodes.Ldstr, node.Name);
            ilProcessor.Emit(OpCodes.Callvirt, _getItemFromDictionary);
        }

        private void EmitNumberLiteralExpression(BoundNumberDoubleLiteralExpression node, ILProcessor ilProcessor)
        {
            ilProcessor.Emit(OpCodes.Ldc_R8, node.Value);
        }

        private void EmitStringLiteralExpression(BoundStringLiteralExpression node, ILProcessor ilProcessor)
        {
            ilProcessor.Emit(OpCodes.Ldstr, node.Value);
        }

        private BoundExpression? ConvertExpression(BoundExpression expression, TypeSymbol targetType)
        {
            var conversion = Conversion.Classify(expression.Type, targetType);
            if (!conversion.Exists)
            {
                return null;
            }

            if (conversion.IsIdentity)
            {
                return expression;
            }
            else
            {
                return BoundNodeFactory.Conversion(expression.Syntax, targetType, expression);
            }

        }

        private BoundExpression RewriteFunctionCall(BoundFunctionCallExpression node, TypedFunctionSymbol function)
        {
            var numberOfArguments = node.Arguments.Length;
            if (numberOfArguments != function.Parameters.Length)
            {
                throw new NotImplementedException($"Function '{function.Name}' expected {function.Parameters.Length} arguments, but was called with {numberOfArguments}.");
            }

            var rewrittenArguments = ImmutableArray.CreateBuilder<BoundExpression>(numberOfArguments);
            for (var i = 0; i < numberOfArguments; i++)
            {
                var argument = node.Arguments[i];
                var parameter = function.Parameters[i];
                var rewrittenArgument = ConvertExpression(argument, parameter.Type);
                if (rewrittenArgument is null)
                {
                    throw new NotImplementedException($"Argument number {i + 1} of function '{function.Name}' expects {parameter.Type}, but got {argument.Type}, and no conversion exists.");

                }
                rewrittenArguments.Add(rewrittenArgument);
            }
            
            return BoundNodeFactory.TypedFunctionCall(node.Syntax, function, rewrittenArguments.MoveToImmutable());
        }

        private void EmitTypedFunctionCallExpression(BoundTypedFunctionCallExpression node, ILProcessor ilProcessor)
        {
            var function = _builtInFunctions[node.Function];
            foreach (var argument in node.Arguments)
            {
                EmitExpression(argument, ilProcessor);
            }

            ilProcessor.Emit(OpCodes.Call, function);
            if (node.Function.ReturnType == TypeSymbol.Void)
            {
                ilProcessor.Emit(OpCodes.Ldnull);
            }
            else if (node.Function.ReturnType == TypeSymbol.MObject)
            {
            }
            else
            {
                throw new Exception("Don't know how to cast function output.");
            }
        }

        private void EmitFunctionCallExpression(BoundFunctionCallExpression node, ILProcessor ilProcessor)
        {
            if (node.Name.Kind == BoundNodeKind.IdentifierNameExpression
                && ((BoundIdentifierNameExpression)node.Name).Name == "disp")
            {
                var dispFunctionSymbol = BuiltInFunctions.Disp;
                var rewrittenCall = RewriteFunctionCall(node, dispFunctionSymbol);
                EmitExpression(rewrittenCall, ilProcessor);
                //ilProcessor.Emit(OpCodes.Call, _dispReference);
                //ilProcessor.Emit(OpCodes.Ldnull);
            } else if (node.Name.Kind == BoundNodeKind.IdentifierNameExpression
                && ((BoundIdentifierNameExpression)node.Name).Name == "len")
            {
                EmitExpression(node.Arguments[0], ilProcessor);
                ilProcessor.Emit(OpCodes.Call, _lenReference);
            }
            else if (node.Name.Kind == BoundNodeKind.TypedVariableExpression)
            {
                if (node.Arguments.Length > 1)
                {
                    throw new Exception("Multi-dimensional array slicing is not supported.");
                }

                var typedVariableExpression = (BoundTypedVariableExpression)node.Name;
                EmitTypedVariableExpression(typedVariableExpression, ilProcessor);
                EmitExpression(node.Arguments[0], ilProcessor);
                ilProcessor.Emit(OpCodes.Call, _arraySliceReference);
            }
            else
            {
                var function = ResolveFunction(node.Name);
                for (var i = 0; i < function.InputDescription.Length; i++)
                {
                    if (i < node.Arguments.Length)
                    {
                        EmitExpression(node.Arguments[i], ilProcessor);
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

        private MethodInterfaceDescription ResolveFunction(BoundExpression node)
        {
            if (node.Kind == BoundNodeKind.IdentifierNameExpression)
            {
                var name = ((BoundIdentifierNameExpression)node).Name;
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
                throw new NotImplementedException($"Dynamic functions calling not supported. Failed to resolve function call expression with kind '{node.Kind}'.");
            }
        }
    }
}
