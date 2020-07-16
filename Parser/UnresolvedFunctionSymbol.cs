namespace Parser
{
    internal class UnresolvedFunctionSymbol
    {
        public string Name { get; }

        public UnresolvedFunctionSymbol(string name)
        {
            Name = name;
        }
    }
}