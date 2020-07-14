using Parser.Objects;

namespace Parser.MFunctions
{
    public static class MOperations
    {
        public static MObject? Plus(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateDoubleNumber(lValue + rValue);
            }

            return null;
        }

        public static MObject? Minus(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateDoubleNumber(lValue - rValue);
            }

            return null;
        }

        public static MObject? Star(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateDoubleNumber(lValue * rValue);
            }

            return null;
        }
        public static MObject? Slash(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateDoubleNumber(lValue / rValue);
            }

            return null;
        }
    }
}
