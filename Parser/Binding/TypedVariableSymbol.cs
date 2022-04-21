namespace Parser.Binding
{
    public class TypedVariableSymbol
    {
        public TypedVariableSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public TypeSymbol Type { get; }
    }
}
