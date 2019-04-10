using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Internal
{
    internal abstract class SyntaxNode : GreenNode
    {
        protected SyntaxNode(TokenKind kind) : base(kind)
        {
        }

        protected SyntaxNode(TokenKind kind, TokenDiagnostic[] diagnostics)
            : base(kind, diagnostics)
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
        protected StatementSyntaxNode(TokenKind kind) : base(kind)
        { 
        }

        protected StatementSyntaxNode(TokenKind kind, TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
        {
        }
    }

    internal abstract class ExpressionSyntaxNode : SyntaxNode
    {
        protected ExpressionSyntaxNode(TokenKind kind) : base(kind)
        {
        }

        protected ExpressionSyntaxNode(TokenKind kind, TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
        {
        }
    }

    internal abstract class FunctionHandleSyntaxNode : ExpressionSyntaxNode
    {
        protected FunctionHandleSyntaxNode(TokenKind kind) : base(kind)
        {
        }

        protected FunctionHandleSyntaxNode(TokenKind kind, TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
        {
        }
    }

    internal abstract class MethodDeclarationSyntaxNode : StatementSyntaxNode
    {
        protected MethodDeclarationSyntaxNode(TokenKind kind) : base(kind)
        {
        }

        protected MethodDeclarationSyntaxNode(TokenKind kind, TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
        {
        }
    }

    internal class RootSyntaxNode : SyntaxNode
    {
        internal readonly FileSyntaxNode _file;

        public RootSyntaxNode(FileSyntaxNode file) : base(TokenKind.Root)
        {
            Slots = 1;
            this.AdjustWidth(file);
            _file = file;
        }

        public RootSyntaxNode(FileSyntaxNode file, TokenDiagnostic[] diagnostics) : base(TokenKind.Root, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(file);
            _file = file;
        }

        public override GreenNode? GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _file;
                default: return null;
            }
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new RootSyntaxNode(this._file, diagnostics);
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.RootSyntaxNode(this, position);
        }
    }
}
