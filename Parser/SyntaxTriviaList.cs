using Parser.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Parser
{
    public struct SyntaxTriviaList : IReadOnlyList<SyntaxTrivia>
    {
        internal GreenNode? Node { get; }

        internal SyntaxTriviaList(SyntaxToken token, GreenNode? node, int position)
        {
            Node = node;
            Token = token;
            Position = position;
        }

        public SyntaxToken Token { get; }

        public int Position { get; }

        public int Count => (Node as SyntaxList<Internal.SyntaxTrivia>)?.Length ?? 0;

        public int Width => Node?.FullWidth ?? 0;

        public SyntaxTrivia this[int index]
        {
            get
            {
                return Node switch
                {
                    SyntaxList<Internal.SyntaxTrivia> triviaList => new SyntaxTrivia(Token.Parent, triviaList[index]),
                    _ => throw new IndexOutOfRangeException(),
                };
            }
        }

        public IEnumerator<SyntaxTrivia> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}