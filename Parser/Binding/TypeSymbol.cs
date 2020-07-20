namespace Parser.Binding
{
    public class TypeSymbol
    {
        public static readonly TypeSymbol Error = new TypeSymbol("error");
        public static readonly TypeSymbol Null = new TypeSymbol("null");
        public static readonly TypeSymbol Boolean = new TypeSymbol("bool");
        public static readonly TypeSymbol Double = new TypeSymbol("double");
        public static readonly TypeSymbol Int = new TypeSymbol("int");
        public static readonly TypeSymbol String = new TypeSymbol("string");
        public static readonly TypeSymbol MObject = new TypeSymbol("mobject");
        public static readonly TypeSymbol Void = new TypeSymbol("void");

        private TypeSymbol(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
