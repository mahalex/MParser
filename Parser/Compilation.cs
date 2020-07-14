using Parser.Binding;

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

        private BoundProgram GetBoundProgram()
        {
            return Binder.BindProgram(_syntaxTree);
        }

        public EvaluationResult Evaluate(CompilationContext context)
        {
            var program = GetBoundProgram();
            if (program.Diagnostics.Length > 0)
            {
                return new EvaluationResult(null, program.Diagnostics);
            }

            var evaluator = new Evaluator(program, context);
            return evaluator.Evaluate();
        }
    }
}