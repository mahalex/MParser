using Mono.Cecil;
using Parser.Binding;
using System.Collections.Immutable;

namespace Parser.Emitting
{
    public class MethodInterfaceDescription
    {
        public MethodInterfaceDescription(ImmutableArray<ParameterSymbol> inputDescription, ImmutableArray<ParameterSymbol> outputDescription, MethodDefinition method)
        {
            InputDescription = inputDescription;
            OutputDescription = outputDescription;
            Method = method;
        }

        public ImmutableArray<ParameterSymbol> InputDescription { get; }

        public ImmutableArray<ParameterSymbol> OutputDescription { get; }

        public MethodDefinition Method { get; }
    }
}
