using Parser.Internal;
using Parser.Objects;
using System.Collections.Immutable;

namespace Parser
{
    public class EvaluationResult
    {
        public EvaluationResult(MObject? value, ImmutableArray<Diagnostic> diagnostics)
        {
            Value = value;
            Diagnostics = diagnostics;
        }

        public MObject? Value { get; }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
    }
}