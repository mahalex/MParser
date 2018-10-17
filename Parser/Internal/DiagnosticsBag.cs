using System.Collections;
using System.Collections.Generic;

namespace Parser.Internal
{
    public class DiagnosticsBag : IEnumerable<Diagnostic>
    {
        internal DiagnosticsBag()
        {
            _diagnostics = new List<Diagnostic>();
        }

        private readonly List<Diagnostic> _diagnostics;

        public IReadOnlyCollection<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();

        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }

        internal void ReportUnexpectedEndOfFile(TextSpan span)
        {
            Report(span, "Unexpected end of file.");
        }

        public IEnumerator<Diagnostic> GetEnumerator()
        {
            return _diagnostics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}