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

        public EvaluationResult Evaluate(CompilationContext context)
        {
            var evaluator = new Evaluator(_syntaxTree, context);
            return evaluator.Evaluate();
        }
    }
}