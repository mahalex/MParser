namespace Parser
{
    public class MParser
    {
        private readonly ITextWindow _window;
        
        public MParser(ITextWindow window)
        {
            _window = window;
        }

        public FileSyntaxNode Parse()
        {
            var lexer = new Internal.MLexerGreen(_window);
            var tokens = lexer.ParseAll();
            var parser = new Internal.MParserGreen(tokens, new Internal.SyntaxFactory());
            var green = parser.ParseFile();
            return new FileSyntaxNode(null, green);            
        }
    }
}