using System.Collections.Generic;

namespace Parser.Internal
{
    internal class SyntaxListBuilder
    {
        private readonly List<GreenNode> _list;
        
        public SyntaxListBuilder()
        {
            _list = new List<GreenNode>();
        }

        public void Add(GreenNode node)
        {
            _list.Add(node);
        }

        public void AddRange(SyntaxList list)
        {
            for (var i = 0; i < list.Slots; i++)
            {
                var element = list.GetSlot(i);
                _list.Add(element);
            }
        }

        public SyntaxList ToList()
        {
            return _list.Count == 0 ? null : SyntaxList.List(_list.ToArray());
        }
    }
}