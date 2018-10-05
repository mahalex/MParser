using System.Collections.Generic;
using Parser;

namespace Semantics
{
    public class MClass
    {
        public string Name { get; }
        public string FileName { get; }
        public FileSyntaxNode Tree { get; }
        public List<MMethod> Methods { get; }

        public MClass(FileSyntaxNode tree, string name, string fileName, List<MMethod> methods)
        {
            Tree = tree;
            Name = name;
            FileName = FileName;
            Methods = methods;
        }
    }
}