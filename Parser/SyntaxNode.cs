﻿using Parser.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public abstract class SyntaxNode
    {
        private readonly SyntaxNode _parent;
        internal readonly Internal.GreenNode _green;
        internal SyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position)
        {
            _parent = parent;
            _green = green;
            Position = position;
            FullSpan = new TextSpan(Position, green.FullWidth);
        }

        private protected SyntaxNode(Internal.GreenNode green, int position)
        {
            _parent = this;
            _green = green;
            Position = position;
            FullSpan = new TextSpan(Position, green.FullWidth);
        }

        public TokenKind Kind => _green.Kind;

        public SyntaxNode Parent => _parent;

        public int Slots => _green.Slots;

        public ChildNodesAndTokensList GetChildNodesAndTokens()
        {
            return new ChildNodesAndTokensList(this);
        }
        
        public int Position { get; }

        public TextSpan FullSpan { get; }

        public TextSpan Span => CalculateSpan();

        private TextSpan CalculateSpan()
        {
            var leadingTriviaWidth = LeadingTrivia?.Width ?? 0;
            var trailingTriviaWidth = TrailingTrivia?.Width ?? 0;
            return new TextSpan(Position + leadingTriviaWidth, _green.FullWidth - leadingTriviaWidth - trailingTriviaWidth);
        }

        public int FullWidth => _green.FullWidth;

        internal int GetChildPosition(int slot)
        {
            var result = Position;
            while (slot > 0)
            {
                slot--;
                var greenChild = _green.GetSlot(slot);
                if (greenChild != null)
                {
                    result += greenChild.FullWidth;
                }
            }
            return result;
        }

        internal abstract SyntaxNode? GetNode(int index);

        internal SyntaxNode? GetRed(ref SyntaxNode? field, int slot)
        {
            if (field == null)
            {
                var green = _green.GetSlot(slot);
                if (green != null)
                {
                    field = green.CreateRed(this, this.GetChildPosition(slot));
                }
            }

            return field;
        }

        public override string ToString()
        {
            return Text;
        }

        public virtual string Text => _green.Text;

        public virtual string FullText => _green.FullText;

        public virtual SyntaxTriviaList? LeadingTrivia
        {
            get
            {
                return GetFirstToken()?.LeadingTrivia;
            }
        }

        public virtual SyntaxTriviaList? TrailingTrivia
        {
            get
            {
                return GetLastToken()?.TrailingTrivia;
            }
        }

        public SyntaxToken? GetFirstToken()
        {
            return SyntaxNavigator.Singleton.EnumerateTokens(this).Select(t => (SyntaxToken?)t).FirstOrDefault();
        }

        public SyntaxToken? GetLastToken()
        {
            return SyntaxNavigator.Singleton.EnumerateTokens(this).Select(t => (SyntaxToken?)t).LastOrDefault();
        }

        public abstract void Accept(SyntaxVisitor visitor);

        public SyntaxDiagnostic[] GetDiagnostics()
        {
            return GetDiagnosticsRecursive(_green, Position).ToArray();
        }

        private static IEnumerable<SyntaxDiagnostic> GetDiagnosticsRecursive(Internal.GreenNode node, int position)
        {
            if (node.HasDiagnostics)
            {
                foreach (var diagnostic in node.GetDiagnostics())
                {
                    yield return SyntaxDiagnostic.From(diagnostic, position);
                }
            }

            for (var i = 0; i < node.Slots; i++)
            {
                var maybeChild = node.GetSlot(i);
                if (maybeChild is Internal.GreenNode child) {
                    foreach (var diagnostic in GetDiagnosticsRecursive(child, position))
                    {
                        yield return diagnostic;
                    }

                    position += child.FullWidth;
                }
            }
        }
    }
    
    public abstract class StatementSyntaxNode : SyntaxNode
    {
        internal StatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position) : base(parent, green, position)
        {
        }
    }
    
    public abstract class ExpressionSyntaxNode : SyntaxNode
    {
        internal ExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position) : base(parent, green, position)
        {
        }
    }

    public abstract class FunctionHandleExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal FunctionHandleExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position) : base(parent, green, position)
        {
        }
    }

    public abstract class MethodDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal MethodDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position) : base(parent, green, position)
        {
        }
    }

    public class RootSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _file;

        internal RootSyntaxNode(Internal.GreenNode green, int position) : base(green, position)
        {
        }

        internal override SyntaxNode? GetNode(int index)
        {
            return index switch
            {
                0 => GetRed(ref _file!, 0),
                _ => null,
            };
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitRoot(this);
        }


        public FileSyntaxNode File
        {
            get
            {
                var red = this.GetRed(ref this._file, 0);
                if (red != null)
                    return (FileSyntaxNode)red;

                throw new System.Exception("file cannot be null");
            }
        }
    }
}