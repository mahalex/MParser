namespace Parser.Objects
{
    public class MCharArray : MObject
    {
        private MCharArray(char[] chars)
        {
            Chars = chars;
        }

        public char[] Chars { get; }

        public static MCharArray Create(char[] chars)
        {
            return new MCharArray(chars);
        }

        public override string ToString()
        {
            return new string(Chars);
        }
    }
}