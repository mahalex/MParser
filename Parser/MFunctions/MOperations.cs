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

        public static MObject? Greater(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateLogical(lValue > rValue);
            }

            return null;
        }

        public static MObject? GreaterOrEquals(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateLogical(lValue >= rValue);
            }

            return null;
        }

        public static MObject? Less(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateLogical(lValue < rValue);
            }

            return null;
        }

        public static MObject? LessOrEquals(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue })
            {
                return MObject.CreateLogical(lValue <= rValue);
            }

            return null;
        }

        public static MObject? Minus(MObject operand)
        {
            if (operand is MDoubleNumber { Value: var value })
            {
                return MObject.CreateDoubleNumber(-value);
            }

            return null;
        }
    }
}
