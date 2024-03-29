﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Parser.Internal
{
    internal class SyntaxTrivia : GreenNode
    {
        private readonly string _text;
        
        public SyntaxTrivia(TokenKind kind, string text) : base(kind, text.Length)
        {
            _text = text;
        }

        public SyntaxTrivia(TokenKind kind, string text, TokenDiagnostic[] diagnostics) : base(kind, text.Length, diagnostics)
        {
            _text = text;
        }

        public override string Text => _text;
        public override string FullText => _text;
        public int Width => _text.Length;

        public override GreenNode? GetSlot(int i)
        {
            throw new System.NotImplementedException();
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            throw new InvalidOperationException();
        }

        public override bool IsTrivia => true;
        public override bool IsNode => false;

        public override void WriteTriviaTo(TextWriter writer)
        {
            writer.Write(_text);
        }

        public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
        {
            writer.Write(_text);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new SyntaxTrivia(Kind, _text, diagnostics);
        }

        public override GreenNode? LeadingTriviaCore => null;
        public override GreenNode? TrailingTriviaCore => null;
    }
}