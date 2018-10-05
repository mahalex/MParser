using System;
using System.Collections.Generic;

namespace Parser.Internal
{
    internal class SyntaxList<T> : SyntaxNode where T : GreenNode
    {
        private readonly SyntaxList _list;
        
        protected SyntaxList(T[] list) : base(TokenKind.List, list.Length)
        {
            _list = SyntaxList.List(list);
        }

        public override GreenNode GetSlot(int i)
        {
            return (T)_list.GetSlot(i);
        }

        public static SyntaxList<T> List(T[] elements)
        {
            return new SyntaxList<T>(elements);
        }

        public override bool IsList => true;

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.SyntaxNodeOrTokenList(parent, this);
        }
    }
}