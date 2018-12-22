namespace Parser.Internal
{
    internal partial class SyntaxFactory
    {
        public RootSyntaxNode RootSyntax(FileSyntaxNode file)
        {
            return new RootSyntaxNode(file);
        }
    }
}