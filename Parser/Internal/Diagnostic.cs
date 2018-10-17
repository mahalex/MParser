using System;

namespace Parser.Internal
{
    public class Diagnostic
    {
        public TextSpan Span { get; }
        public string Message { get; }

        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}