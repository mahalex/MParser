using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Parser
{
    public abstract class SyntaxNode
    {
        private readonly SyntaxNode _parent;
        internal readonly Internal.GreenNode _green; 
        internal SyntaxNode(SyntaxNode parent, Internal.GreenNode green)
        {
            _parent = parent;
            _green = green;
        }

        public TokenKind Kind => _green.Kind;

        public SyntaxNode Parent => _parent;

        public ChildNodesAndTokensList GetChildNodesAndTokens()
        {
            return new ChildNodesAndTokensList(this);
        }
        
        internal abstract SyntaxNode GetNode(int index);

        internal SyntaxNode GetRed(ref SyntaxNode field, int slot)
        {
            if (field == null)
            {
                var green = _green.GetSlot(slot);
                if (green != null)
                {
                    field = green.CreateRed(this);
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

        public virtual IReadOnlyList<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                var p = Parent;
                return _green.LeadingTrivia.Select(trivia => new SyntaxTrivia(p, trivia)).ToImmutableList();
            }
        }

        public virtual IReadOnlyList<SyntaxTrivia> TrailingTrivia
        {
            get
            {
                var p = Parent;
                return _green.TrailingTrivia.Select(trivia => new SyntaxTrivia(p, trivia)).ToImmutableList();
            }
        }

        public abstract void Accept(SyntaxVisitor visitor);
    }
    
    public abstract class StatementSyntaxNode : SyntaxNode
    {
        internal StatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }
    }
    
    public abstract class ExpressionSyntaxNode : SyntaxNode
    {
        internal ExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }
    }

    public abstract class FunctionHandleSyntaxNode : ExpressionSyntaxNode
    {
        internal FunctionHandleSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }
    }

    public abstract class MethodDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal MethodDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }
    }

}