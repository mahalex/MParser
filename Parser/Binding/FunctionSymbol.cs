using System.Collections.Immutable;

namespace Parser.Binding
{
    public class FunctionSymbol
    {
        public FunctionSymbol(
            string name,
            ImmutableArray<ParameterSymbol> parameters,
            FunctionDeclarationSyntaxNode? declaration)
        {
            Name = name;
            Parameters = parameters;
            Declaration = declaration;
        }

        public string Name { get; }

        public ImmutableArray<ParameterSymbol> Parameters { get; }

        public FunctionDeclarationSyntaxNode? Declaration { get; }
    }
}
