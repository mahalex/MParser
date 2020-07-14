using System.Globalization;

namespace Parser.Objects
{
    public class MDoubleNumber : MObject
    {
        private MDoubleNumber(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public static MDoubleNumber Create(double value)
        {
            return new MDoubleNumber(value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}