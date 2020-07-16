using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static SyntaxGenerator.SyntaxExtensions;

namespace SyntaxGenerator
{
    public class GenerateSyntax
    {
        private const string SyntaxTokenClassName = "SyntaxToken";
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

        private static MemberDeclarationSyntax GenerateInternalFieldDeclaration(FieldDescription field)
        {
            return FieldDeclaration(
                VariableDeclaration(FullFieldType(field))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier(field.GetPrivateFieldName())))))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.InternalKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)));
        }

        private static MemberDeclarationSyntax GeneratePrivateFieldDeclaration(FieldDescription field)
        {
            return
                FieldDeclaration(
                    VariableDeclaration(NullableType(IdentifierName("SyntaxNode")))
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                    Identifier(
                                        field.GetPrivateFieldName())))))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PrivateKeyword)));
        }

        private static IEnumerable<ExpressionStatementSyntax> GenerateFieldAssignmentInsideConstructor(FieldDescription field)
        {
            yield return
                ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName("AdjustWidth")))
                    .WithArgumentList(
                        SingleArgument(
                            IdentifierName(field.FieldName))));
            yield return
                ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(field.GetPrivateFieldName()),
                        IdentifierName(field.FieldName)));
        }

        private static MemberDeclarationSyntax GenerateInternalConstructorSimple(SyntaxNodeDescription node)
        {
            return
                ConstructorDeclaration(
                    Identifier(node.ClassName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.InternalKeyword)))
                .WithParameterList(
                     ParameterList(
                         IntersperseWithCommas(
                             node.Fields.Select(
                                 field =>
                                 Parameter(Identifier(field.FieldName))
                                 .WithType(FullFieldType(field))))))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        SingleArgument(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("TokenKind"),
                                IdentifierName(node.TokenKindName)))))
                .WithBody(
                    Block(
                        node.Fields.SelectMany(GenerateFieldAssignmentInsideConstructor)
                        .Prepend(
                            ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName("Slots"),
                                LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    Literal(node.Fields.Length)))))));
        }

        private static MemberDeclarationSyntax GenerateInternalConstructorWithDiagnostics(SyntaxNodeDescription node)
        {
            return
                ConstructorDeclaration(
                    Identifier(node.ClassName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.InternalKeyword)))
                .WithParameterList(
                     ParameterList(
                         IntersperseWithCommas(
                             node.Fields.Select(
                                 field =>
                                 Parameter(Identifier(field.FieldName))
                                 .WithType(FullFieldType(field)))
                             .Append(
                                 Parameter(
                                     Identifier("diagnostics"))
                                 .WithType(
                                     ArrayType(
                                         IdentifierName("TokenDiagnostic"))
                                     .WithRankSpecifiers(
                                         SingletonList(
                                             ArrayRankSpecifier(
                                                 SingletonSeparatedList<ExpressionSyntax>(
                                                     OmittedArraySizeExpression())))))))))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        SeveralArguments(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("TokenKind"),
                                IdentifierName(node.TokenKindName)),
                            IdentifierName("diagnostics"))))
                .WithBody(
                    Block(
                        node.Fields.SelectMany(GenerateFieldAssignmentInsideConstructor)
                        .Prepend(
                            ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName("Slots"),
                                LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    Literal(node.Fields.Length)))))));
        }

        private static IEnumerable<MemberDeclarationSyntax> GenerateInternalConstructors(SyntaxNodeDescription node)
        {
            yield return GenerateInternalConstructorSimple(node);
            yield return GenerateInternalConstructorWithDiagnostics(node);
        }

        private static MemberDeclarationSyntax GenerateConstructor(SyntaxNodeDescription node)
        {
            return
                ConstructorDeclaration(
                    Identifier(node.ClassName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.InternalKeyword)))
                .WithParameterList(
                    ParameterList(
                        IntersperseWithCommas(
                            Parameter(
                                Identifier("parent"))
                            .WithType(
                                IdentifierName("SyntaxNode")),
                            Parameter(
                                Identifier("green"))
                            .WithType(
                                QualifiedName(
                                    IdentifierName("Internal"),
                                    IdentifierName("GreenNode"))),
                            Parameter(
                                Identifier("position"))
                            .WithType(
                                PredefinedType(
                                    Token(SyntaxKind.IntKeyword))))))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        SeveralArguments(
                            IdentifierName("parent"),
                            IdentifierName("green"),
                            IdentifierName("position"))))
                .WithBody(
                    Block());
        }

        private static MemberDeclarationSyntax GenerateInternalSetDiagnostics(SyntaxNodeDescription node)
        {
            return
                MethodDeclaration(
                    IdentifierName("GreenNode"),
                    Identifier("SetDiagnostics"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.OverrideKeyword)}))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier("diagnostics"))
                            .WithType(
                                ArrayType(
                                    IdentifierName("TokenDiagnostic"))
                                .WithRankSpecifiers(
                                    SingletonList(
                                        ArrayRankSpecifier(
                                            SingletonSeparatedList<ExpressionSyntax>(
                                                OmittedArraySizeExpression()))))))))
                .WithBody(
                    Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                ObjectCreationExpression(
                                    IdentifierName(node.ClassName))
                                .WithArgumentList(
                                    SeveralArguments(
                                        node.Fields.Select(
                                            f => IdentifierName(f.GetPrivateFieldName()))
                                        .Append(IdentifierName("diagnostics"))))))));
        }

        private static MethodDeclarationSyntax GenerateInternalGetSlot(SyntaxNodeDescription node)
        {
            return
                MethodDeclaration(
                    NullableType(
                        IdentifierName("GreenNode")),
                    Identifier("GetSlot"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.OverrideKeyword)}))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier("i"))
                            .WithType(
                                PredefinedType(
                                    Token(SyntaxKind.IntKeyword))))))
                .WithBody(
                    Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                SwitchExpression(
                                    IdentifierName("i"))
                                .WithArms(
                                    IntersperseWithCommas(
                                        node.Fields
                                        .Select((f, i) => SwitchExpressionArm(
                                            ConstantPattern(
                                                LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    Literal(i))),
                                            IdentifierName(f.GetPrivateFieldName())))
                                        .Append(
                                            SwitchExpressionArm(
                                                DiscardPattern(),
                                                LiteralExpression(
                                                    SyntaxKind.NullLiteralExpression)))))))));
        }
        
        private static MemberDeclarationSyntax GenerateGetSlot(List<(FieldDescription field, int index)> pairs)
        {
            static ArgumentSyntax GetFieldNameWithPossibleBang(FieldDescription field)
            {
                var name = IdentifierName(field.GetPrivateFieldName());
                return field.FieldIsNullable
                ? Argument(name)
                : Argument(
                    PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression,
                    name));
            }

            return
                MethodDeclaration(
                    NullableType(
                        IdentifierName("SyntaxNode")),
                    Identifier("GetNode"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.InternalKeyword),
                            Token(SyntaxKind.OverrideKeyword)}))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier("i"))
                            .WithType(
                                PredefinedType(
                                    Token(SyntaxKind.IntKeyword))))))
                                .WithBody(
                                    Block(
                                        SingletonList<StatementSyntax>(
                                            ReturnStatement(
                                                SwitchExpression(
                                                    IdentifierName("i"))
                                                .WithArms(
                                                    IntersperseWithCommas(
                                                        pairs.Select(pair =>
                                                            SwitchExpressionArm(
                                                                ConstantPattern(
                                                                    LiteralExpression(
                                                                        SyntaxKind.NumericLiteralExpression,
                                                                        Literal(pair.index))),
                                                                InvocationExpression(
                                                                    IdentifierName("GetRed"))
                                                                .WithArgumentList(
                                                                    ArgumentList(
                                                                        IntersperseWithCommas(
                                                                            GetFieldNameWithPossibleBang(pair.field)
                                                                            .WithRefKindKeyword(
                                                                                Token(SyntaxKind.RefKeyword)),
                                                                            Argument(
                                                                                LiteralExpression(
                                                                                    SyntaxKind.NumericLiteralExpression,
                                                                                    Literal(pair.index))))))))
                                                        .Append(
                                                            SwitchExpressionArm(
                                                                DiscardPattern(),
                                                                LiteralExpression(
                                                                    SyntaxKind.NullLiteralExpression)))))))));
        }

        private static MemberDeclarationSyntax GenerateCreateRed(SyntaxNodeDescription node)
        {
            return
                MethodDeclaration(
                    QualifiedName(
                        IdentifierName(OuterNamespace),
                        IdentifierName("SyntaxNode")),
                    Identifier("CreateRed"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.InternalKeyword),
                            Token(SyntaxKind.OverrideKeyword)}))
                .WithParameterList(
                    ParameterList(
                        IntersperseWithCommas(
                            Parameter(
                                Identifier("parent"))
                            .WithType(
                                QualifiedName(
                                    IdentifierName(OuterNamespace),
                                    IdentifierName("SyntaxNode"))),
                            Parameter(
                                Identifier("position"))
                            .WithType(
                                PredefinedType(
                                    Token(SyntaxKind.IntKeyword))))))
                .WithBody(
                    Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                ObjectCreationExpression(
                                    QualifiedName(
                                        IdentifierName(OuterNamespace),
                                        IdentifierName(node.ClassName)))
                                .WithArgumentList(
                                    SeveralArguments(
                                        IdentifierName("parent"),
                                        ThisExpression(),
                                        IdentifierName("position")))))));
        }

        private static MemberDeclarationSyntax GenerateInternalClass(SyntaxNodeDescription node)
        {
            var classDeclaration =
                ClassDeclaration(node.ClassName)
                .WithModifiers(
                    TokenList(Token(SyntaxKind.InternalKeyword)));
            if (node.BaseClassName != null)
            {
                classDeclaration = classDeclaration
                    .WithBaseList(
                        BaseList(
                            SingletonSeparatedList<BaseTypeSyntax>(
                                SimpleBaseType(IdentifierName(node.BaseClassName)))));
            }

            return classDeclaration
                 .WithMembers(
                     List(
                         node.Fields
                         .Select(GenerateInternalFieldDeclaration)
                         .Concat(GenerateInternalConstructors(node))
                         .Append(GenerateCreateRed(node))
                         .Append(GenerateInternalSetDiagnostics(node))
                         .Append(GenerateInternalGetSlot(node))));
        }

        private static string Capitalize(string s)
        {
            return s[0].ToString().ToUpper() + s[1..];
        }

        private static MemberDeclarationSyntax GenerateTokenAccessor(SyntaxNodeDescription node, FieldDescription field, int index)
        {
            return
                PropertyDeclaration(
                    IdentifierName("SyntaxToken"),
                    Identifier(Capitalize(field.FieldName)))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    AccessorList(
                        SingletonList(
                            AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ReturnStatement(
                                            ObjectCreationExpression(
                                                IdentifierName("SyntaxToken"))
                                            .WithArgumentList(
                                                SeveralArguments(
                                                    ThisExpression(),
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        ParenthesizedExpression(
                                                            CastExpression(
                                                                QualifiedName(
                                                                    GetInternalNamespace(),
                                                                    IdentifierName(node.ClassName)),
                                                                IdentifierName("_green"))),
                                                        IdentifierName(field.GetPrivateFieldName())),
                                                    InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            ThisExpression(),
                                                            IdentifierName("GetChildPosition")))
                                                    .WithArgumentList(
                                                        SingleArgument(
                                                            LiteralExpression(
                                                                SyntaxKind.NumericLiteralExpression,
                                                                Literal(index)))))))))))));
}

        private static bool IsList(string type)
        {
            return type.StartsWith("SyntaxList");
        }

        private static MemberDeclarationSyntax GenerateNodeAccessor(FieldDescription field, int index)
        {
            var type = field.FieldType;
            if (IsList(type))
            {
                type = "SyntaxNodeOrTokenList";
            }

            return
                PropertyDeclaration(
                    field.FieldIsNullable ? (TypeSyntax)NullableType(IdentifierName(type)) : IdentifierName(type),
                    Identifier(Capitalize(field.FieldName)))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    AccessorList(
                        SingletonList(
                            AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithBody(
                                Block(
                                    LocalDeclarationStatement(
                                        VariableDeclaration(
                                            IdentifierName("var"))
                                        .WithVariables(
                                            SingletonSeparatedList(
                                                VariableDeclarator(
                                                    Identifier("red"))
                                                .WithInitializer(
                                                    EqualsValueClause(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                ThisExpression(),
                                                                IdentifierName("GetRed")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                IntersperseWithCommas(
                                                                    Argument(
                                                                        field.FieldIsNullable
                                                                        ? (ExpressionSyntax)MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                ThisExpression(),
                                                                                IdentifierName(field.GetPrivateFieldName()))
                                                                        : PostfixUnaryExpression(
                                                                            SyntaxKind.SuppressNullableWarningExpression,
                                                                            MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                ThisExpression(),
                                                                                IdentifierName(field.GetPrivateFieldName()))))
                                                                    .WithRefKindKeyword(
                                                                        Token(SyntaxKind.RefKeyword)),
                                                                    Argument(
                                                                        LiteralExpression(
                                                                            SyntaxKind.NumericLiteralExpression,
                                                                            Literal(index))))))))))),
                                    ReturnStatement(
                                        ConditionalExpression(
                                            IsPatternExpression(
                                                IdentifierName("red"),
                                                ConstantPattern(
                                                    LiteralExpression(
                                                        SyntaxKind.NullLiteralExpression))),

                                            field.FieldIsNullable
                                            ? (ExpressionSyntax)LiteralExpression(
                                                SyntaxKind.DefaultLiteralExpression,
                                                Token(SyntaxKind.DefaultKeyword))
                                            : ThrowExpression(
                                                ObjectCreationExpression(
                                                    QualifiedName(
                                                        IdentifierName("System"),
                                                        IdentifierName("Exception")))
                                                .WithArgumentList(
                                                    SingleArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal($"{field.FieldName} cannot be null."))))),
                                            CastExpression(
                                                IdentifierName(type),
                                                IdentifierName("red")))))))));
        }

        private static string ConvertClassNameToVisitorName(string name)
        {
            if (name.EndsWith("SyntaxNode"))
            {
                name = name.Substring(0, name.Length - "SyntaxNode".Length);
            }

            return "Visit" + name;
        }

        private static MemberDeclarationSyntax GenerateAccessMethod(SyntaxNodeDescription node)
        {
            var visitorName = ConvertClassNameToVisitorName(node.ClassName);
            Visitors.Add((visitorName, node.ClassName));
            return
                MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier("Accept"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.OverrideKeyword)}))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier("visitor"))
                            .WithType(
                                IdentifierName("SyntaxVisitor")))))
                .WithBody(
                    Block(
                        SingletonList<StatementSyntax>(
                            ExpressionStatement(
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("visitor"),
                                        IdentifierName(visitorName)))
                                .WithArgumentList(
                                    SingleArgument(
                                        ThisExpression()))))));
        }

        private static MemberDeclarationSyntax GenerateClass(SyntaxNodeDescription node)
        {
            var classDeclaration =
                ClassDeclaration(node.ClassName)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)));
            if (node.BaseClassName != null)
            {
                classDeclaration = classDeclaration
                    .WithBaseList(
                        BaseList(
                            SingletonSeparatedList<BaseTypeSyntax>(
                                SimpleBaseType(IdentifierName(node.BaseClassName)))));
            }
            var tokenSlots = node.Fields
                .Select((f, i) => (field: f, index: i))
                .Where(pair => pair.field.FieldType == SyntaxTokenClassName)
                .ToList();
            var nodeSlots = node.Fields
                .Select((f, i) => (field: f, index: i))
                .Where(pair => pair.field.FieldType != SyntaxTokenClassName)
                .ToList();

            return classDeclaration
                 .WithMembers(
                     List(
                         nodeSlots
                         .Select(pair => GeneratePrivateFieldDeclaration(pair.field))
                         .Append(GenerateConstructor(node))
                         .Concat(tokenSlots.Select(pair => GenerateTokenAccessor(node, pair.field, pair.index)))
                         .Concat(nodeSlots.Select(pair => GenerateNodeAccessor(pair.field, pair.index)))
                         .Append(GenerateGetSlot(nodeSlots))
                         .Append(GenerateAccessMethod(node))));
        }

        private static string GenerateInternalSyntaxNodeFile(SyntaxDescription syntax)
        {
            return GenerateInternalSyntaxNodeCompilationUnit(syntax).NormalizeWhitespace().ToFullString();
        }

        private static NameSyntax GetInternalNamespace()
        {
            return QualifiedName(
                IdentifierName("Parser"),
                IdentifierName("Internal"));
        }

        private static NamespaceDeclarationSyntax GetOuterNamespace()
        {
            return NamespaceDeclaration(
                IdentifierName("Parser"));
        }

        private static CompilationUnitSyntax GenerateInternalSyntaxNodeCompilationUnit(SyntaxDescription syntax)
        {
            return CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                            GetInternalNamespace())
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    Trivia(
                                        NullableDirectiveTrivia(
                                            Token(SyntaxKind.EnableKeyword),
                                            true))),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()))
                        .WithMembers(
                            List(
                                syntax.Nodes.Select(GenerateInternalClass)))));
        }

        private static string GenerateSyntaxNodeFile(SyntaxDescription syntax)
        {
            return GenerateSyntaxNodeCompilationUnit(syntax).NormalizeWhitespace().ToFullString();
        }

        private static CompilationUnitSyntax GenerateSyntaxNodeCompilationUnit(SyntaxDescription syntax)
        {
            return CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        GetOuterNamespace()
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    Trivia(
                                        NullableDirectiveTrivia(
                                            Token(SyntaxKind.EnableKeyword),
                                            true))),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()))
                        .WithMembers(
                            List(
                                syntax.Nodes.Select(GenerateClass)))));
        }

        private static string FactoryMethodNameFromClassName(string className)
        {
            return className.EndsWith("Node") ? className[0..^4] : className;
        }

        private static TypeSyntax FullFieldType(FieldDescription field)
        {
            return field.FieldIsNullable 
                ? (TypeSyntax)NullableType(IdentifierName(field.FieldType))
                : IdentifierName(field.FieldType);
        }

        private static MemberDeclarationSyntax GenerateFactoryMethod(SyntaxNodeDescription node)
        {
            return
                MethodDeclaration(
                    IdentifierName(node.ClassName),
                    Identifier(FactoryMethodNameFromClassName(node.ClassName)))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        IntersperseWithCommas(
                            node.Fields.Select(
                                field => Parameter(
                                    Identifier(field.FieldName))
                                .WithType(FullFieldType(field))))))
                .WithBody(
                    Block(
                        SingletonList<StatementSyntax>(
                            ReturnStatement(
                                ObjectCreationExpression(
                                    IdentifierName(node.ClassName))
                                .WithArgumentList(
                                    SeveralArguments(
                                        node.Fields.Select(
                                            field => IdentifierName(field.FieldName))))))));
        }

        private static string GenerateSyntaxFactoryFile(SyntaxDescription syntax)
        {
            return GenerateSyntaxFactoryCompilationUnit(syntax).NormalizeWhitespace().ToFullString();
        }

        private static CompilationUnitSyntax GenerateSyntaxFactoryCompilationUnit(SyntaxDescription syntax)
        {
            return CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                            GetInternalNamespace())
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    Trivia(
                                        NullableDirectiveTrivia(
                                            Token(SyntaxKind.EnableKeyword),
                                            true))),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration("SyntaxFactory")
                                .WithModifiers(
                                    TokenList(
                                        Token(SyntaxKind.InternalKeyword),
                                        Token(SyntaxKind.PartialKeyword)))
                                .WithMembers(
                                    List(
                                        syntax.Nodes.Select(GenerateFactoryMethod)))))));
        }

        private static MemberDeclarationSyntax GenerateVisitor((string visitorMethodName, string className) info)
        {
            return
                MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier(info.visitorMethodName))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.VirtualKeyword)}))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier("node"))
                            .WithType(
                                IdentifierName(info.className)))))
                .WithBody(
                    Block(
                        SingletonList(
                            ExpressionStatement(
                                InvocationExpression(
                                    IdentifierName("DefaultVisit"))
                                .WithArgumentList(
                                    SingleArgument(
                                        IdentifierName("node")))))));
        }

        private static string GenerateSyntaxVisitorFile()
        {
            return GenerateSyntaxVisitorCompilationUnit().NormalizeWhitespace().ToFullString();
        }

        private static CompilationUnitSyntax GenerateSyntaxVisitorCompilationUnit()
        {
            return
                CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        GetOuterNamespace()
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    Trivia(
                                        NullableDirectiveTrivia(
                                            Token(SyntaxKind.EnableKeyword),
                                            true))),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList()))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration("SyntaxVisitor")
                                .WithModifiers(
                                    TokenList(
                                        Token(SyntaxKind.PublicKeyword),
                                        Token(SyntaxKind.PartialKeyword)))
                                .WithMembers(
                                    List(
                                        Visitors.Select(GenerateVisitor)))))));

        }

        public static void Input()
        {
            var serializer = new XmlSerializer(typeof(SyntaxDescription));
            var syntaxDefinitionFileName = Path.Combine(_outputPath, "SyntaxDefinition.xml");
            using var stream = new FileStream(syntaxDefinitionFileName, FileMode.Open);
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
            File.WriteAllText(syntaxVisitorPath, GenerateSyntaxVisitorFile());
        }
        
        static void Main()
        {
            Console.Write("Generating syntax...");
            Input();
            Console.WriteLine("Done.");
        }
    }
}