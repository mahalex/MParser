using Parser.Objects;
using System.Collections.Generic;

namespace Parser
{
    internal class EvaluationScope
    {
        public Dictionary<string, MObject> Variables { get; } = new Dictionary<string, MObject>();
    }
}