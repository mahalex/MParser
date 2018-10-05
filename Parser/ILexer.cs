using System.Collections.Generic;

namespace Parser
{
    public interface ILexer<T>
    {
        T NextToken();
        List<T> ParseAll();
    }
}
