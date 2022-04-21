using Parser.Binding;
using Parser.Emitting;

namespace Parser
{
    public class Compilation
    {
        private readonly SyntaxTree _syntaxTree;

        private Compilation(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
        }

        public static Compilation Create(SyntaxTree syntaxTree)
        {
            return new Compilation(syntaxTree);
        }

        public void Emit(string[] references, string outputPath)
        {
            var emitter = new Emitter();
            var boundProgram = GetBoundProgram();
            emitter.Emit(boundProgram, references, outputPath);
        }

        private BoundProgram GetBoundProgram()
        {
            return Binder.BindProgram(_syntaxTree);
        }

        public EvaluationResult Evaluate(CompilationContext context, bool inRepl)
        {
            var program = GetBoundProgram();
            if (program.Diagnostics.Length > 0)
            {
                return new EvaluationResult(null, program.Diagnostics);
            }

            var evaluator = new Evaluator(program, context, inRepl);
            return evaluator.Evaluate();
        }
    }
}