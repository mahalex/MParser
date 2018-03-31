using System;

namespace Lexer
{
    public class ParsingException : Exception
    {
        public ParsingException(string text) : base(text) {}
    }
}