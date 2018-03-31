using System.Collections.Generic;

namespace Lexer
{
    public interface ILexer<T> where T : class
    {
        T NextToken();
        List<T> ParseAll();
    }
}
