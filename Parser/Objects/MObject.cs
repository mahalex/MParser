namespace Parser.Objects
{
    public abstract class MObject
    {
        public static MDoubleNumber CreateDoubleNumber(double value)
        {
            return MDoubleNumber.Create(value);
        }
    }
}