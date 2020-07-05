using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SyntaxGenerator
{
    public static class SyntaxExtensions
    {
        public static ArgumentListSyntax SingleArgument(ExpressionSyntax expression)
        {
            return
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(expression)));
        }

        public static ArgumentListSyntax SeveralArguments(IEnumerable<ExpressionSyntax> expressions)
        {
            return ArgumentList(
                IntersperseWithCommas(
                    expressions.Select(e => Argument(e))));
        }

        public static ArgumentListSyntax SeveralArguments(params ExpressionSyntax[] expressions)
        {
            return SeveralArguments((IEnumerable<ExpressionSyntax>)expressions);
        }

        public static IEnumerable<SyntaxNodeOrToken> IntersperseWithCommas(IEnumerable<SyntaxNodeOrToken> tokens)
        {
            var first = true;
            foreach (var token in tokens)
            {
                if (first)
                {
                    yield return token;
                    first = false;
                }
                else
                {
                    yield return Token(SyntaxKind.CommaToken);
                    yield return token;
                }
            }
        }

        public static SeparatedSyntaxList<TNode> IntersperseWithCommas<TNode>(IEnumerable<TNode> tokens)
            where TNode : SyntaxNode
        {
            return SeparatedList<TNode>(
                IntersperseWithCommas(tokens.Select(token => (SyntaxNodeOrToken)token)));
        }

        public static SeparatedSyntaxList<TNode> IntersperseWithCommas<TNode>(params TNode[] tokens)
            where TNode : SyntaxNode
        {
            return IntersperseWithCommas((IEnumerable<TNode>)tokens);
        }
    }
}