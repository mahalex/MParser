using Parser.Internal;
using System.Collections.Immutable;

namespace Parser.Binding
{
    public class BoundProgram
    {
        public BoundProgram(BoundRoot nullRoot, ImmutableArray<Diagnostic> diagnostics)
        {
            NullRoot = nullRoot;
            Diagnostics = diagnostics;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        public BoundRoot NullRoot { get; }

        public BoundFile Root => NullRoot.File;
    }
}
