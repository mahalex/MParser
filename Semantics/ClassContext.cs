using System.Collections.Generic;

namespace Semantics
{
    internal class ClassContext
    {
        public string Path { get; }
        public Dictionary<string, ClassContext> SubClasses { get; }
        public Dictionary<string, FunctionContext> Methods { get; }
        public Dictionary<string, FunctionContext> PrivateMethods { get; }

        public ClassContext(
            string path,
            Dictionary<string, ClassContext> subClasses,
            Dictionary<string, FunctionContext> methods,
            Dictionary<string, FunctionContext> privateMethods)
        {
            Path = path;
            SubClasses = subClasses;
            Methods = methods;
            PrivateMethods = privateMethods;
        }

        public ClassContext(string path)
        {
            Path = path;
            SubClasses = new Dictionary<string, ClassContext>();
            Methods = new Dictionary<string, FunctionContext>();
            PrivateMethods = new Dictionary<string, FunctionContext>();
        }
    }
}