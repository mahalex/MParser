using System.Linq;

namespace Parser.Internal
{
    internal class SyntaxList<T> : SyntaxNode where T : GreenNode
    {
        private readonly SyntaxList _list;
        
        protected SyntaxList(T[] list) : base(TokenKind.List)
        {
            Slots = list.Length;
            _list = SyntaxList.List(list);
            foreach (var element in list)
            {
                this.AdjustWidth(element);
            }
        }

        protected SyntaxList(T[] list, TokenDiagnostic[] diagnostics) : base(TokenKind.List, diagnostics)
        {
            Slots = list.Length;
            _list = SyntaxList.List(list);
            foreach (var element in list)
            {
                this.AdjustWidth(element);
            }
        }

        public override GreenNode? GetSlot(int i)
        {
            return (T)_list.GetListSlot(i);
        }

        public static SyntaxList<T> List(T[] elements)
        {
            return new SyntaxList<T>(elements);
        }

        public static SyntaxList<T> Empty => new SyntaxList<T>(new T[] { });

        public override bool IsList => true;

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.SyntaxNodeOrTokenList(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new SyntaxList<T>(_list._elements.Select(x => (T)x).ToArray(), diagnostics);
        }
    }
}