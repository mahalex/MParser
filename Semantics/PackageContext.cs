using System.Collections.Generic;

namespace Semantics
{
    internal class PackageContext
    {
        public string Path { get; }
        public Dictionary<string, PackageContext> SubPackages { get; }
        public Dictionary<string, ClassContext> Classes { get; }
        public Dictionary<string, FunctionContext> Functions { get; }

        public PackageContext(
            string path,
            Dictionary<string, PackageContext> subPackages,
            Dictionary<string, ClassContext> classes,
            Dictionary<string, FunctionContext> functions)
        {
            Path = path;
            SubPackages = subPackages;
            Classes = classes;
            Functions = functions;
        }

        public PackageContext(
            string path)
        {
            Path = path;
            SubPackages = new Dictionary<string, PackageContext>();
            Classes = new Dictionary<string, ClassContext>();
            Functions = new Dictionary<string, FunctionContext>();
        }
    }
}