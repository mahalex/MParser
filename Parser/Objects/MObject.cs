namespace Parser.Objects
{
    public abstract class MObject
    {
        public static MDoubleNumber CreateDoubleNumber(double value)
        {
            return MDoubleNumber.Create(value);
        }

        public static MCharArray CreateCharArray(char[] chars)
        {
            return MCharArray.Create(chars);
        }

        public static MLogical CreateLogical(bool value)
        {
            return MLogical.Create(value);
        }
    }
}