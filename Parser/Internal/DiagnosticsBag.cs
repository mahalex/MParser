using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Internal
{
    public class DiagnosticsBag : IEnumerable<Diagnostic>
    {
        internal DiagnosticsBag()
        {
            _diagnostics = new List<Diagnostic>();
        }

        public DiagnosticsBag(IEnumerable<Diagnostic> diagnostics)
        {
            _diagnostics = diagnostics.ToList();
        }

        private readonly List<Diagnostic> _diagnostics;

        public IReadOnlyCollection<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();

        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }

        private void Report(string message)
        {
            var diagnostic = new Diagnostic(message);
            _diagnostics.Add(diagnostic);
        }

        internal void ReportUnexpectedEndOfFile(TextSpan span)
        {
            Report(span, "Unexpected end of file.");
        }

        internal void ReportNotEnoughInputs(TextSpan span, string functionName)
        {
            Report(span, $"Not enough inputs for function '{functionName}'.");
        }

        internal void ReportUnexpectedCharacterWhileParsingNumber(TextSpan span, char c)
        {
            Report(span, $"Unexpected character '{c}' while parsing a number.");
        }

        internal void ReportUnexpectedEOLWhileParsingString(TextSpan span)
        {
            Report(span, "Unexpected end of line while parsing a string literal.");
        }

        internal void ReportUnknownSymbol(TextSpan span, char c)
        {
            Report(span, $"Unknown symbol '{c}'.");
        }

        internal void ReportUnexpectedToken(TokenKind expected, TokenKind actual)
        {
            Report($"Unexpected token '{actual}', expected '{expected}'.");
        }

        internal void ReportUnmatchedCloseParenthesis(TextSpan span, TokenKind kind)
        {
            Report(span, $"Unmatched close parenthesis '{kind}'.");
        }

        internal void ReportUnmatchedOpenParenthesisByEndOfFile(TextSpan span)
        {
            Report(span, "Unmatched open parenthesis by the end of file.");
        }

        internal void ReportCannotEvaluateExpression(TextSpan span)
        {
            Report(span, $"Cannot evaluate expression.");
        }

        public IEnumerator<Diagnostic> GetEnumerator()
        {
            return _diagnostics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void ReportVariableNotFound(TextSpan span, string variableName)
        {
            Report(span, $"Variable '{variableName}' not found.");
        }

        internal void ReportFunctionNotFound(TextSpan span, string functionName)
        {
            Report(span, $"Function '{functionName}' not found.");
        }

        internal void ReportTooManyInputs(TextSpan span, string functionName)
        {
            Report(span, $"Too many inputs in the call to '{functionName}'.");
        }
    }
}