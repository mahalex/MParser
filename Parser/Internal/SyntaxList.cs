using System;
using System.Collections.Generic;

namespace Parser.Internal
{
    internal class SyntaxList : SyntaxNode
    {
        private readonly GreenNode[] _elements;
        
        protected SyntaxList(GreenNode[] elements) : base(TokenKind.List)
        {
            Slots = elements.Length;
            _elements = elements;
            foreach (var element in elements)
            {
                this.AdjustWidth(element);
            }
        }

        public GreenNode GetListSlot(int i)
        {
            return _elements[i];
        }

        public override GreenNode? GetSlot(int i)
        {
            return GetListSlot(i);
        }

        public static SyntaxList List(GreenNode[] elements)
        {
            return new SyntaxList(elements);
        }

        public static SyntaxList EmptyList { get; } = new SyntaxList(new GreenNode[] { });

        public override bool IsList => true;

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.SyntaxNodeOrTokenList(parent, this, position);
        }
    }
}