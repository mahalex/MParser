using System;
using System.Collections;
using System.Collections.Generic;
using Parser.Internal;

namespace Parser
{
    public class ChildNodesAndTokensList : IReadOnlyList<SyntaxNodeOrToken>
    {
        private readonly SyntaxNode _node;
        private readonly int _count;
        
        internal ChildNodesAndTokensList(SyntaxNode node)
        {
            _node = node;
            _count = CountChildNodes(node._green);
        }

        private int CountChildNodes(GreenNode green)
        {
            var counter = 0;
            for (var i = 0; i < green.Slots; i++)
            {
                var child = green.GetSlot(i);
                if (child == null)
                {
                    continue;
                }

                if (child.IsList)
                {
                    counter += child.Slots;
                }
                else
                {
                    counter++;
                }
            }

            return counter;
        }
        
        public IEnumerator<SyntaxNodeOrToken> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _count;

        public SyntaxNodeOrToken this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return ThisInternal(index);
                
            }
        }

        internal SyntaxNodeOrToken ThisInternal(int index)
        {
            var currentSlotIndex = 0;
            GreenNode? currentSlot = null;
            while (true)
            {
                currentSlot = _node._green.GetSlot(currentSlotIndex);
                if (currentSlot == null)
                {
                    currentSlotIndex++;
                    continue;
                }

                var nodesInCurrentSlot = currentSlot.IsList ? currentSlot.Slots : 1;
                if (index < nodesInCurrentSlot)
                {
                    if (currentSlot.IsList)
                    {
                        var listSlot = _node.GetNode(currentSlotIndex);
                        if (listSlot is null)
                        {
                            throw new Exception($"Unexpected null in list slot.");
                        }
                        var red = listSlot.GetNode(index);
                        if (!(red is null))
                        {
                            return red;
                        }
                        // this is a token
                        return new SyntaxToken(listSlot, listSlot._green.GetSlot(index)!, _node.GetChildPosition(index));
                    }
                    else
                    {
                        var red = _node.GetNode(currentSlotIndex);
                        if (red != null)
                        {
                            return red;
                        }
                        // this is a token
                        return new SyntaxToken(_node, _node._green.GetSlot(currentSlotIndex)!, _node.GetChildPosition(currentSlotIndex));
                    }
                }

                index -= nodesInCurrentSlot;
                currentSlotIndex++;
            }
        }

        private struct Enumerator : IEnumerator<SyntaxNodeOrToken>
        {
            private int _index;
            private readonly ChildNodesAndTokensList _list;
            private readonly int _count;

            internal Enumerator(ChildNodesAndTokensList list)
            {
                _index = -1;
                _list = list;
                _count = _list.Count;
            }
            
            public bool MoveNext()
            {
                _index++;
                return _index < _count;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public SyntaxNodeOrToken Current => _list[_index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}