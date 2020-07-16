using System.Collections.Immutable;

namespace Parser.Binding
{
    public class LoweredFunction {
        public LoweredFunction(
            BoundFunctionDeclaration declaration,
            string name,
            ImmutableArray<ParameterSymbol> inputDescription,
            ImmutableArray<ParameterSymbol> outputDescription,
            BoundBlockStatement body)
        {
            Declaration = declaration;
            Name = name;
            InputDescription = inputDescription;
            OutputDescription = outputDescription;
            Body = body;
        }

        public BoundFunctionDeclaration Declaration { get; }
        public string Name { get; }
        public ImmutableArray<ParameterSymbol> InputDescription { get; }
        public ImmutableArray<ParameterSymbol> OutputDescription { get; }
        public BoundBlockStatement Body { get; }
    }
}
