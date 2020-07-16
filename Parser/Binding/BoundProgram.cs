using Parser.Internal;
using System.Collections.Immutable;

namespace Parser.Binding
{
    public class BoundProgram
    {
        public BoundProgram(
            ImmutableArray<Diagnostic> diagnostics,
            FunctionSymbol? mainFunction,
            FunctionSymbol? scriptFunction,
            ImmutableDictionary<FunctionSymbol, BoundBlockStatement> functions)
        {
            Diagnostics = diagnostics;
            MainFunction = mainFunction;
            ScriptFunction = scriptFunction;
            Functions = functions;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        /// <summary>
        /// A "main" function (first in a file without any global statements).
        /// </summary>
        public FunctionSymbol? MainFunction { get; }

        /// <summary>
        /// A "script" function (generated from all global statements in a file if there are any).
        /// </summary>
        public FunctionSymbol? ScriptFunction { get; }

        /// <summary>
        /// So-called "local" functions.
        /// </summary>
        public ImmutableDictionary<FunctionSymbol, BoundBlockStatement> Functions { get; }
    }
}
