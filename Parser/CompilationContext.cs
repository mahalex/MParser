using Parser.Objects;
using System.Collections.Generic;

namespace Parser
{
    public class CompilationContext
    {
        public Dictionary<string, MObject> Variables { get; } = new Dictionary<string, MObject>();
        public static CompilationContext Empty => new CompilationContext();
    }
}