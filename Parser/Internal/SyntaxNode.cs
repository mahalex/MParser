using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Internal
{
    internal abstract class SyntaxNode : GreenNode
    {
        protected SyntaxNode(TokenKind kind, int slots) : base(kind, slots)
        {
        }

        public IEnumerable<SyntaxToken> DescendantTokens => CalculateChildTokens();

        private IEnumerable<SyntaxToken> CalculateChildTokens()
        {
            for (var i = 0; i < Slots; i++)
            {
                var slot = GetSlot(i);
                switch (slot)
                {
                    case null:
                        continue;
                    case SyntaxToken token:
                        yield return token;
                        break;
                    case SyntaxNode node:
                        foreach (var t in node.DescendantTokens)
                        {
                            yield return t;
                        }

                        break;
                }
            }
        }

        protected virtual string CollectFullText()
        {
            var builder = new StringBuilder();
            foreach (var token in DescendantTokens)
            {
                builder.Append(token.FullText);
            }

            return builder.ToString();
        }

        public override string FullText => CollectFullText();

        public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore => throw new NotImplementedException();
        public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore => throw new NotImplementedException();
    }

    internal abstract class StatementSyntaxNode : SyntaxNode
    {
        protected StatementSyntaxNode(TokenKind kind, int slots) : base(kind, slots + 1)
        {
        }
    }
    
    internal abstract class ExpressionSyntaxNode : SyntaxNode
    {
        protected ExpressionSyntaxNode(TokenKind kind, int slots) : base(kind, slots)
        {
        }
    }

    internal abstract class FunctionHandleSyntaxNode : ExpressionSyntaxNode
    {
        protected FunctionHandleSyntaxNode(TokenKind kind, int slots) : base(kind, slots)
        {
        }
    }

    internal abstract class MethodDeclarationSyntaxNode : StatementSyntaxNode
    {
        protected MethodDeclarationSyntaxNode(TokenKind kind, int slots) : base(kind, slots)
        {
        }
    }
}
