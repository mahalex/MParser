using Parser;

namespace MApplication
{
    internal static class CodeProcessor
    {
        public static DisplayText GetText(SyntaxTree tree)
        {
            var visitor = new ColoringVisitor(StyleSchemeFactory.GetDefaultScheme());
            visitor.Visit(tree.Root);
            return new DisplayText(visitor.GetLines());
        }
    }
}
