using System.Globalization;

namespace Parser.Objects
{
    public class MLogical : MObject
    {
        public MLogical(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public static MLogical Create(bool value)
        {
            return new MLogical(value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}