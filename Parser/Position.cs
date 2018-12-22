namespace Parser
{
    public struct Position
    {
        public string? FileName { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int Offset { get; set; }

        public override string ToString()
        {
            return $"line {Line}, column {Column} of {FileName}.";
        }
    }
}