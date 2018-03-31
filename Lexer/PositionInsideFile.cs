namespace Lexer
{
    public struct PositionInsideFile : IPosition
    {
        public string File { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString()
        {
            return $"line {Line}, column {Column}" + (File != null ? $" of {File}" : "");
        }
    }
}