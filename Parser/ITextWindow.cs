﻿namespace Parser
{
    public interface ITextWindow
    {
        bool IsEof();
        char PeekChar();
        char PeekChar(int n);
        void ConsumeChar();
        void ConsumeChars(int n);
        char GetAndConsumeChar();
        string GetAndConsumeChars(int n);
        int CharactersLeft();
        Position Position { get; }
    }
}