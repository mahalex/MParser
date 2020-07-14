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

            public SyntaxTokenWithTrivia(
                TokenKind kind,
                string text,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia,
                TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
            {
                _text = text;
                LeadingTriviaCore = leadingTrivia;
                TrailingTriviaCore = trailingTrivia;
                _fullWidth = (leadingTrivia?.Sum(t => t.FullWidth) ?? 0) + (text?.Length ?? 0) + (trailingTrivia?.Sum(t => t.FullWidth) ?? 0);
            }

            public SyntaxTokenWithTrivia(
                TokenKind kind,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia,
                TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
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

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new SyntaxTokenWithTrivia(Kind, _text, LeadingTrivia, TrailingTrivia, diagnostics);
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

            public SyntaxTokenWithValue(
                TokenKind kind,
                string text,
                T value,
                TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
            {
                _text = text;
                _value = value;
                _fullWidth = text?.Length ?? 0;
            }


            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                writer.Write(_text);
            }

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new SyntaxTokenWithValue<T>(Kind, _text, _value, diagnostics);
            }

            public T TypedValue => _value;

            public override object? Value => TypedValue;
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

            public SyntaxTokenWithValueAndTrivia(
                TokenKind kind,
                string text,
                T value,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia,
                TokenDiagnostic[] diagnostics) : base(kind, text, value, diagnostics)
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

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new SyntaxTokenWithValueAndTrivia<T>(Kind, _text, TypedValue, LeadingTrivia, TrailingTrivia, diagnostics);
            }
        }

        internal class SyntaxIdentifier : SyntaxToken
        {
            private readonly string _text;

            public override void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
            {
                writer.Write(_text);
            }

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new SyntaxIdentifier(_text, diagnostics);
            }

            public SyntaxIdentifier(
                string text) : base(TokenKind.IdentifierToken)
            {
                _text = text;
                _fullWidth = text?.Length ?? 0;
            }

            public SyntaxIdentifier(
                string text,
                TokenDiagnostic[] diagnostics) : base(TokenKind.IdentifierToken, diagnostics)
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

            public SyntaxIdentifierWithTrivia(
                string text,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia,
                TokenDiagnostic[] diagnostics) : base(text, diagnostics)
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

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new SyntaxIdentifierWithTrivia(Text, _leadingTrivia, _trailingTrivia, diagnostics);
            }
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

            public MissingTokenWithTrivia(
                TokenKind kind,
                IReadOnlyList<SyntaxTrivia> leadingTrivia,
                IReadOnlyList<SyntaxTrivia> trailingTrivia,
                TokenDiagnostic[] diagnostics) : base(kind, leadingTrivia, trailingTrivia, diagnostics)
            {
                _isMissing = true;
            }

            public override string Text => "";

            public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
            {
                return new MissingTokenWithTrivia(Kind, LeadingTrivia, TrailingTrivia, diagnostics);
            }
        }

        protected SyntaxToken(TokenKind kind) : base(kind)
        {
        }

        protected SyntaxToken(TokenKind kind, TokenDiagnostic[] diagnostics) : base(kind, diagnostics)
        {
        }

        internal static SyntaxToken NoneToken => new MissingTokenWithTrivia(TokenKind.None, s_EmptySyntaxTriviaList, s_EmptySyntaxTriviaList);

        public virtual object? Value => null;

        public override object? GetValue() => Value;

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