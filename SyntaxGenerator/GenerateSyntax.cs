﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxGenerator
{
    public class GenerateSyntax
    {
        private const string SyntaxTokenClassName = "SyntaxToken";
        private const string InternalNamespace = "Parser.Internal";
        private const string OuterNamespace = "Parser";

        private static readonly List<(string visitorMethodName, string className)> Visitors = new List<(string, string)>();

        private static readonly string _outputPath;

        static GenerateSyntax()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    _outputPath = Path.Combine("..", "Parser");
                    break;
                default:
                    _outputPath = Path.Combine("..", "..", "..", "..", "Parser");
                    break;
            }
        }

        public static void TestOutput()
        {
            var field1 = new FieldDescription
            {
                FieldType = "SyntaxToken",
                FieldName = "functionKeyword"
            };
            var field2 = new FieldDescription
            {
                FieldType = "FunctionOutputDescriptionSyntaxNode",
                FieldName = "outputDescription"
            };
            var syntaxNode = new SyntaxNodeDescription
            {
                ClassName = "FunctionDeclarationSyntaxNode",
                BaseClassName = "StatementSyntaxNode",
                Fields = new[] {field1, field2}
            };
            var syntax = new SyntaxDescription
            {
                Nodes = new[] {syntaxNode, syntaxNode}
            };
            var serializer = new XmlSerializer(typeof(SyntaxDescription));
            using (var writer = new StreamWriter("output.xml"))
            {
                serializer.Serialize(writer, syntax);
            }            
        }

        private static string GenerateInternalFieldDeclaration(FieldDescription field)
        {
            return $"        internal readonly {FullFieldType(field)} _{field.FieldName};\n";
        }

        private static string GeneratePrivateFieldDeclaration(FieldDescription field)
        {
            //var typeDeclaration = field.FieldIsNullable ? "SyntaxNode?" : "SyntaxNode";
            var typeDeclaration = "SyntaxNode?";
            return $"        private {typeDeclaration} _{field.FieldName};\n";
        }

        private static string GenerateFieldAssignmentInsideConstructor(FieldDescription field)
        {
            var widthAdjustment = $"            this.AdjustWidth({field.FieldName});\n";
            var fieldAssignment = $"            _{field.FieldName} = {field.FieldName};\n";
            return widthAdjustment + fieldAssignment;
        }

        private static string GenerateInternalConstructorSimple(SyntaxNodeDescription node)
        {
            var arguments = string.Join(
                ",",
                node.Fields.Select(field => $"\n            {FullFieldType(field)} {field.FieldName}"));

            var header =
                $"        internal {node.ClassName}({arguments}) : base(TokenKind.{node.TokenKindName})";
            var slotsAssignment = $"            Slots = {node.Fields.Length};";
            var assignments = string.Join(
                "",
                node.Fields.Select(GenerateFieldAssignmentInsideConstructor));
            return header + "\n        {\n" + slotsAssignment + "\n" + assignments + "        }\n";
        }

        private static string GenerateInternalConstructorWithDiagnostics(SyntaxNodeDescription node)
        {
            var arguments = string.Join(
                ",",
                node.Fields
                    .Select(field => $"\n            {FullFieldType(field)} {field.FieldName}")
                    .Concat(new[] { $"\n            TokenDiagnostic[] diagnostics" }));
            var header =
                $"        internal {node.ClassName}({arguments}) : base(TokenKind.{node.TokenKindName}, diagnostics)";
            var slotsAssignment = $"            Slots = {node.Fields.Length};";
            var assignments = string.Join(
                "",
                node.Fields.Select(GenerateFieldAssignmentInsideConstructor));
            return header + "\n        {\n" + slotsAssignment + "\n" + assignments + "        }\n";
        }

        private static string GenerateInternalConstructors(SyntaxNodeDescription node)
        {
            return GenerateInternalConstructorSimple(node) + "\n" + GenerateInternalConstructorWithDiagnostics(node);
        }

        private static string GenerateConstructor(SyntaxNodeDescription node)
        {
            var arguments = "SyntaxNode parent, Internal.GreenNode green, int position";
            var header =
                $"        internal {node.ClassName}({arguments}) : base(parent, green, position)\n";
            return header + "        {\n        }\n";
        }

        private static string GenerateInternalSetDiagnostics(SyntaxNodeDescription node)
        {
            var header = $"        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)";
            var arguments = string.Join(
                ", ",
                node.Fields
                    .Select(field => "_" + field.FieldName)
                    .Concat(new[] { "diagnostics" }));
            var text = $"            return new {node.ClassName}({arguments});";
            return header + "\n        {\n" + text + "\n        }\n";
        }

        private static string GenerateInternalGetSlot(SyntaxNodeDescription node)
        {
            var header = $"        public override GreenNode? GetSlot(int i)\n";
            var cases = string.Join(
                "",
                node.Fields.Select((f, i) => $"                case {i}: return _{f.FieldName};\n"));
            var defaultCase = "                default: return null;\n";
            return header
                   + "        {\n            switch (i)\n            {\n"
                   + cases
                   + defaultCase
                   + "            }\n"
                   + "        }\n";
        }
        
        private static string GenerateGetSlot(SyntaxNodeDescription node, List<(FieldDescription field, int index)> pairs)
        {
            var header = $"        internal override SyntaxNode? GetNode(int i)\n";

            string GetFieldNameWithPossibleBang(FieldDescription field)
            {
                return field.FieldIsNullable ? field.FieldName : field.FieldName + "!";
            }

            var cases = string.Join(
                "",
                pairs.Select(pair => $"                case {pair.index}: return GetRed(ref _{GetFieldNameWithPossibleBang(pair.field)}, {pair.index});\n"));
            var defaultCase = "                default: return null;\n";
            return header
                   + "        {\n            switch (i)\n            {\n"
                   + cases
                   + defaultCase
                   + "            }\n"
                   + "        }\n";
        }

        private static string GenerateCreateRed(SyntaxNodeDescription node)
        {
            var header = $"        internal override {OuterNamespace}.SyntaxNode CreateRed({OuterNamespace}.SyntaxNode parent, int position)\n";
            var text = $"            return new {OuterNamespace}.{node.ClassName}(parent, this, position);\n";
            return header + "        {\n" + text + "        }\n";
        }

        private static string GenerateInternalClass(SyntaxNodeDescription node)
        {
            var header = $"    internal class {node.ClassName}";
            if (node.BaseClassName != null)
            {
                header += $" : {node.BaseClassName}";
            }

            var fields = string.Join(
                "",
                node.Fields.Select(GenerateInternalFieldDeclaration));
            var constructor = GenerateInternalConstructors(node);
            var getSlot = GenerateInternalGetSlot(node);
            var createRed = GenerateCreateRed(node);
            var setDiagnostics = GenerateInternalSetDiagnostics(node);
            return
                header
                + "\n    {\n"
                + fields + "\n"
                + constructor + "\n"
                + createRed + "\n"
                + setDiagnostics + "\n"
                + getSlot + "    }\n";
        }

        private static string Capitalize(string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1, s.Length - 1);
        }

        private static string GenerateTokenAccessor(SyntaxNodeDescription node, FieldDescription field, int index)
        {
            var header = $"        public SyntaxToken {Capitalize(field.FieldName)}\n";
            var text =
                $"            get {{ return new SyntaxToken(this, (({InternalNamespace}.{node.ClassName})_green)._{field.FieldName}, this.GetChildPosition({index})); }}";
            return header + "        {\n" + text + "\n        }\n";
        }

        private static bool IsList(string type)
        {
            return type.StartsWith("SyntaxList");
        }

        private static string GenerateNodeAccessor(SyntaxNodeDescription node, FieldDescription field, int index)
        {
            var type = field.FieldType;
            if (IsList(type))
            {
                type = "SyntaxNodeOrTokenList";
            }

            var typeName = type + (field.FieldIsNullable ? "?" : "");
            var header = $"        public {typeName} {Capitalize(field.FieldName)}\n        {{\n            get\n            {{\n";
            var defaultReturnStatement = field.FieldIsNullable ? $"return default({type});" : $"throw new System.Exception(\"{field.FieldName} cannot be null\");";

            var fieldNameWithPossibleBang = field.FieldIsNullable ? field.FieldName : field.FieldName + "!";
            var text =
                $"                var red = this.GetRed(ref this._{fieldNameWithPossibleBang}, {index});\n"
                + $"                if (red != null)\n"
                + $"                    return ({type})red;\n\n"
                + $"                {defaultReturnStatement}\n";
            return header + text + "            }\n        }\n";
        }

        private static string ConvertClassNameToVisitorName(string name)
        {
            if (name.EndsWith("SyntaxNode"))
            {
                name = name.Substring(0, name.Length - "SyntaxNode".Length);
            }

            return "Visit" + name;
        }

        private static string GenerateAccessMethod(SyntaxNodeDescription node)
        {
            var visitorName = ConvertClassNameToVisitorName(node.ClassName);
            Visitors.Add((visitorName, node.ClassName));
            var header = $"        public override void Accept(SyntaxVisitor visitor)\n";
            var body = $"            visitor.{visitorName}(this);\n";
            return header + "        {\n" + body + "        }\n";
        }

        private static string GenerateClass(SyntaxNodeDescription node)
        {
            var header = $"    public class {node.ClassName}";
            if (node.BaseClassName != null)
            {
                header += $" : {node.BaseClassName}";
            }

            var tokenSlots = node.Fields
                .Select((f, i) => (field: f, index: i))
                .Where(pair => pair.field.FieldType == SyntaxTokenClassName)
                .ToList();
            var nodeSlots = node.Fields
                .Select((f, i) => (field: f, index: i))
                .Where(pair => pair.field.FieldType != SyntaxTokenClassName)
                .ToList();
            var fields = string.Join(
                "",
                nodeSlots.Select(pair => GeneratePrivateFieldDeclaration(pair.field)));
            var constructor = GenerateConstructor(node);
            var tokenAccessors =
                string.Join(
                    "\n",
                    tokenSlots.Select(pair => GenerateTokenAccessor(node, pair.field, pair.index)));
            var nodeAccessors =
                string.Join(
                    "\n",
                    nodeSlots.Select(pair => GenerateNodeAccessor(node, pair.field, pair.index)));
            var getSlot = GenerateGetSlot(node, nodeSlots);
            var access = GenerateAccessMethod(node);
            return
                header
                + "\n    {\n"
                + fields + "\n"
                + constructor + "\n"
                + tokenAccessors + "\n"
                + nodeAccessors + "\n"
                + getSlot + "\n"
                + access + "\n"
                + "    }\n";
        }

        private static string GenerateInternalSyntaxNodeFile(SyntaxDescription syntax)
        {
            var header = $"#nullable enable\nnamespace {InternalNamespace}\n";
            var classes = string.Join(
                "\n",
                syntax.Nodes.Select(GenerateInternalClass)
            );
            return header + "{\n" + classes + "}\n";
        }

        private static string GenerateSyntaxNodeFile(SyntaxDescription syntax)
        {
            var header = $"#nullable enable\nnamespace {OuterNamespace}\n";
            var classes = string.Join(
                "\n",
                syntax.Nodes.Select(GenerateClass)
            );
            return header + "{\n" + classes + "}\n";
        }

        private static string FactoryMethodNameFromClassName(string className)
        {
            if (className.EndsWith("Node"))
            {
                return className.Substring(0, className.Length - 4);
            }
            else
            {
                return className;
            }
        }

        private static string FullFieldType(FieldDescription field)
        {
            return field.FieldIsNullable ? field.FieldType + "?" : field.FieldType;
        }

        private static string GenerateFactoryMethod(SyntaxNodeDescription node)
        {
            var methodName = FactoryMethodNameFromClassName(node.ClassName);
            var header = $"        public {node.ClassName} {methodName}";
            var arguments = string.Join(
                ", ",
                node.Fields.Select(field => $"\n            {FullFieldType(field)} {field.FieldName}"));
            var constructorParameters = string.Join(
                ", ",
                node.Fields.Select(field => $"\n                {field.FieldName}"));
            var returnStatement =
                $"            return new {node.ClassName}({constructorParameters});\n";

            return header + "(" + arguments + ")\n        {\n" + returnStatement + "        }\n";
        }

        private static string GenerateSyntaxFactoryFile(SyntaxDescription syntax)
        {
            var header = $"#nullable enable\nnamespace {InternalNamespace}\n{{\n    internal partial class SyntaxFactory\n";
            var methods = string.Join(
                "\n",
                syntax.Nodes.Select(GenerateFactoryMethod)
            );
            return header + "    {\n" + methods + "    }\n}";
        }

        private static string GenerateVisitor((string visitorMethodName, string className) info)
        {
            var header = $"        public virtual void {info.visitorMethodName}({info.className} node)\n";
            var body = $"            DefaultVisit(node);\n";
            return header + "        {\n" + body + "        }\n";
        }

        private static string GenerateSyntaxVisitorFile(SyntaxDescription syntax)
        {
            var header = $"#nullable enable\nnamespace {OuterNamespace}\n{{\n    public partial class SyntaxVisitor\n";
            var methods = string.Join(
                "\n",
               Visitors.Select(GenerateVisitor));
            return header + "    {\n" + methods + "    }\n};";
        }


        public static void Input()
        {
            var serializer = new XmlSerializer(typeof(SyntaxDescription));
            using (var stream = new FileStream("input.xml", FileMode.Open))
            {
                if (!(serializer.Deserialize(stream) is SyntaxDescription syntax))
                {
                    Console.WriteLine("Couldn't deserialize syntax.");
                    return;
                }

                var internalSyntaxNodePath = Path.Combine(_outputPath, "Internal", "SyntaxNode.Generated.cs");
                File.WriteAllText(internalSyntaxNodePath, GenerateInternalSyntaxNodeFile(syntax));
                var internalSyntaxFactoryPath = Path.Combine(_outputPath, "Internal", "SyntaxFactory.Generated.cs");
                File.WriteAllText(internalSyntaxFactoryPath, GenerateSyntaxFactoryFile(syntax));
                var syntaxNodePath = Path.Combine(_outputPath, "SyntaxNode.Generated.cs");
                File.WriteAllText(syntaxNodePath, GenerateSyntaxNodeFile(syntax));
                var syntaxVisitorPath = Path.Combine(_outputPath, "SyntaxVisitor.Generated.cs");
                File.WriteAllText(syntaxVisitorPath, GenerateSyntaxVisitorFile(syntax));
            }            
        }
        
        static void Main(string[] args)
        {
            Console.Write("Generating syntax...");
            Input();
            Console.WriteLine("Done.");
        }
    }
}