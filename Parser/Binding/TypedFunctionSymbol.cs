using System.Collections.Immutable;

namespace Parser.Binding
{
    public class TypedFunctionSymbol
    {
        public TypedFunctionSymbol(
            string name,
            ImmutableArray<TypedParameterSymbol> parameters,
            TypeSymbol returnType)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
        }

        public string Name { get; }

        public ImmutableArray<TypedParameterSymbol> Parameters { get; }

        public TypeSymbol ReturnType { get; }
    }
}
