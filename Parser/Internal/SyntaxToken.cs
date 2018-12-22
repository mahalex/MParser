using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parser.Internal
{
    internal abstract class SyntaxToken : GreenNode
    {
        internal static IReadOnlyList<SyntaxTrivia> s_EmptySyntaxTriviaList = new List<SyntaxTrivia>();

        internal class SyntaxTokenWithTrivia : SyntaxToken
        {
            private readonly string _text;
            
            public SyntaxTokenWithTrivia(
                TokenKind kind,
                string text,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia) : base(kind)
            {
                _text = text;
                LeadingTriviaCore = leadingTrivia;
                TrailingTriviaCore = trailingTrivia;
                _fullWidth = (leadingTrivia?.Sum(t => t.FullWidth) ?? 0) + (text?.Length ?? 0) + (trailingTrivia?.Sum(t => t.FullWidth) ?? 0);
            }

            public SyntaxTokenWithTrivia(
                TokenKind kind,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia) : base(kind)
            {
                _text = base.Text;
                LeadingTriviaCore = leadingTrivia;
                TrailingTriviaCore = trailingTrivia;
                _fullWidth = (leadingTrivia?.Sum(t => t.FullWidth) ?? 0) + (_text?.Length ?? 0) + (trailingTrivia?.Sum(t => t.FullWidth) ?? 0);
            }

            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                if (leading)
                {
                    foreach (var trivia in LeadingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
                base.WriteTokenTo(writer, leading, trailing);
                if (trailing)
                {
                    foreach (var trivia in TrailingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
            }

            public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore { get; }

            public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore { get; }
        }

        internal class SyntaxTokenWithValue<T> : SyntaxToken
        {
            protected readonly string _text;
            private readonly T _value;

            public SyntaxTokenWithValue(
                TokenKind kind,
                string text,
                T value) : base(kind)
            {
                _text = text;
                _value = value;
                _fullWidth = text?.Length ?? 0;
            }

            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                writer.Write(_text);
            }
            public T Value => _value;
        }

        internal class SyntaxTokenWithValueAndTrivia<T> : SyntaxTokenWithValue<T>
        {
            public SyntaxTokenWithValueAndTrivia(
                TokenKind kind,
                string text,
                T value,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia) : base(kind, text, value)
            {
                LeadingTriviaCore = leadingTrivia;
                TrailingTriviaCore = trailingTrivia;
                _fullWidth = (leadingTrivia?.Sum(t => t.FullWidth) ?? 0) + (text?.Length ?? 0) + (trailingTrivia?.Sum(t => t.FullWidth) ?? 0);
            }

            public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore { get; }

            public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore { get; }
            
            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                if (leading)
                {
                    foreach (var trivia in LeadingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
                base.WriteTokenTo(writer, leading, trailing);
                if (trailing)
                {
                    foreach (var trivia in TrailingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
            }
        }

        internal class SyntaxIdentifier : SyntaxToken
        {
            private readonly string _text;

            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                writer.Write(_text);
            }

            public SyntaxIdentifier(
                string text
                ) : base(TokenKind.IdentifierToken)
            {
                _text = text;
                _fullWidth = text?.Length ?? 0;
            }
        }

        internal class SyntaxIdentifierWithTrivia : SyntaxIdentifier
        {
            private readonly IReadOnlyList<SyntaxTrivia> _leadingTrivia;
            private readonly IReadOnlyList<SyntaxTrivia> _trailingTrivia;
            
            public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore => _leadingTrivia;
            public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore => _trailingTrivia;

            public SyntaxIdentifierWithTrivia(
                string text,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia
                ) : base(text)
            {
                _leadingTrivia = leadingTrivia;
                _trailingTrivia = trailingTrivia;
                _fullWidth = (leadingTrivia?.Sum(t => t.FullWidth) ?? 0) + (text?.Length ?? 0) + (trailingTrivia?.Sum(t => t.FullWidth) ?? 0);
            }

            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                if (leading)
                {
                    foreach (var trivia in LeadingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
                base.WriteTokenTo(writer, leading, trailing);
                if (trailing)
                {
                    foreach (var trivia in TrailingTrivia)
                    {
                        writer.Write(trivia.Text);
                    }
                }
            }

            public override bool IsToken => true;
            public override bool IsNode => false;
        }

        internal class MissingTokenWithTrivia : SyntaxTokenWithTrivia
        {
            public MissingTokenWithTrivia(
                TokenKind kind,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia
                ) : base(kind, leadingTrivia, trailingTrivia)
            {
                _isMissing = true;
            }

            public override string Text => "";
        }

        protected SyntaxToken(TokenKind kind) : base(kind)
        {
        }

        internal static SyntaxToken NoneToken => new MissingTokenWithTrivia(TokenKind.None, s_EmptySyntaxTriviaList, s_EmptySyntaxTriviaList);
        
        public virtual int Width => Text.Length;

        public override IReadOnlyList<SyntaxTrivia> LeadingTriviaCore => s_EmptySyntaxTriviaList;
        public override IReadOnlyList<SyntaxTrivia> TrailingTriviaCore => s_EmptySyntaxTriviaList;

        public override GreenNode? GetSlot(int i)
        {
            throw new System.InvalidOperationException();
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            throw new InvalidOperationException();
        }

        public override bool IsToken => true;
        public override bool IsNode => false;

        public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
        {
            writer.Write(SyntaxFacts.GetText(Kind));
        }
    }
}