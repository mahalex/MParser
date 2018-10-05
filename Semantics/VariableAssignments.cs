using System.Collections.Generic;

namespace Semantics
{
    public class VariableAssignments
    {
        public Context Context { get; }

        private readonly Dictionary<string, Variable> _variables;

        public VariableAssignments()
        {
            _variables = new Dictionary<string, Variable>();
        }

        public Variable Find(string name)
        {
            if (_variables.ContainsKey(name))
            {
                return _variables[name];
            }
            return null;
        }

        public void Add(string name, Variable variable)
        {
            _variables[name] = variable;
        }
    }
}