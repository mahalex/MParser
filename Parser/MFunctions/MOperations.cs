using Parser.Objects;
using System;

namespace Parser.MFunctions
{
    public static class MOperations
    {
        public static MObject? Colon(MObject left, MObject right)
        {
            if (left is MDoubleNumber { Value: var lValue }
                && right is MDoubleNumber { Value: var rValue }) 
            {
                var array = CreateRangeMatrix(lValue, rValue);
                return MObject.CreateDoubleMatrix(array);
            }

            return null;
        }

        private static double[,] CreateRangeMatrix(double lValue, double rValue)
        {
            var length = (int)Math.Round(rValue - lValue) + 1;
            var result = new double[1, length];
            for (var i = 0; i < length; i++)
            {
                result[0, i] = lValue + i;
            }

            return result;
        }

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

        public static MObject? ArraySlice(MObject array, MObject range)
        {
            if (array is MDoubleMatrix m
                && range is MDoubleNumber { Value: var doubleIndex })
            {
                var index = (int)doubleIndex;
                return MObject.CreateDoubleNumber(m[index]);
            }

            return null;
        }
    }
}
