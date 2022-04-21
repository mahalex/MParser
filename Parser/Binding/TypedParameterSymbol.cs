namespace Parser.Binding
{
    public class TypedParameterSymbol
    {
        public TypedParameterSymbol(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public TypeSymbol Type { get; }
    }
}