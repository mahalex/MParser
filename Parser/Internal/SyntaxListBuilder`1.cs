using System.Collections.Generic;

namespace Parser.Internal
{
    internal class SyntaxListBuilder<T> where T : GreenNode
    {
        private readonly List<T> _list;
        
        public SyntaxListBuilder()
        {
            _list = new List<T>();
        }

        public void Add(T node)
        {
            _list.Add(node);
        }

        public SyntaxList<T> ToList()
        {
            return _list.Count == 0 ? SyntaxList<T>.Empty : SyntaxList<T>.List(_list.ToArray());
        }

    }
}