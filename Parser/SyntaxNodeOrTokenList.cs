using System;
using System.Collections;
using System.Collections.Generic;
using Parser.Internal;

namespace Parser
{
    public class SyntaxNodeOrTokenList : SyntaxNode, IReadOnlyCollection<SyntaxNodeOrToken>
    {
        internal SyntaxNodeOrTokenList(SyntaxNode parent, GreenNode green) : base(parent, green)
        {
        }

        public SyntaxNodeOrToken this[int index]
        {
            get
            {
                if (_green != null && index < _green.Slots)
                {
                    var green = _green.GetSlot(index);
                    if (green is Internal.SyntaxToken)
                    {
                        return new SyntaxToken(this, green);
                    }
                    else
                    {
                        return green.CreateRed(this);
                    }
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        internal class Enumerator : IEnumerator<SyntaxNodeOrToken>
        {
            private int _index;
            private readonly SyntaxNodeOrTokenList _list;

            internal Enumerator(SyntaxNodeOrTokenList list)
            {
                _index = -1;
                _list = list;
            }


            public bool MoveNext()
            {
                var newIndex = _index + 1;
                if (newIndex < _list.Count)
                {
                    _index = newIndex;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public SyntaxNodeOrToken Current => _list[_index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
        
        public IEnumerator<SyntaxNodeOrToken> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _green.Slots;
        
        internal override SyntaxNode GetNode(int index)
        {
            if (index < _green.Slots)
            {
                var node = this[index];
                if (node.IsNode)
                {
                    return node.AsNode();
                }
            }

            return null;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitList(this);
        }
    }
}