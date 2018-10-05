using System.Collections.Generic;

namespace Semantics
{
    public class MethodAssignments
    {
        public Context Context { get; }

        private readonly Dictionary<string, Variable> _methods;

        public MethodAssignments()
        {
            _methods = new Dictionary<string, Variable>();
        }

        public Variable Find(string name)
        {
            if (_methods.ContainsKey(name))
            {
                return _methods[name];
            }
            return null;
        }

        public void Add(string name, Variable variable)
        {
            _methods[name] = variable;
        }
    }
}