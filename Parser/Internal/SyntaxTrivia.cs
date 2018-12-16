using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Parser.Internal
{
    internal class SyntaxTrivia : GreenNode
    {
        private readonly string _text;
        
        public SyntaxTrivia(TokenKind kind, string text) : base(kind)
        {
            _text = text;
        }

        public override string Text => _text;
        public int Width => _text.Length;

        public override GreenNode GetSlot(int i)
        {
            throw new System.NotImplementedException();
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            throw new InvalidOperationException();
        }

        public override bool IsTrivia => true;
        public override bool IsNode => false;

        public override void WriteTriviaTo(TextWriter writer)
        {
            writer.Write(_text);
        }

        public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore => new List<SyntaxTrivia>();
        public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore => new List<SyntaxTrivia>();
    }
}