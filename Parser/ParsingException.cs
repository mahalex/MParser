using System;

namespace Parser
{
    public class ParsingException : Exception
    {
        public ParsingException(string text) : base(text) {}
    }
}