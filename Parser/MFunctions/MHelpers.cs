using Parser.Objects;
using System;

namespace Parser.MFunctions
{
    public static class MHelpers
    {
        public static void Disp(MObject? obj)
        {
            if (obj is not null)
            {
                Console.WriteLine(obj);
            }
        }

        public static int Len(MObject obj)
        {
            return obj switch
            {
                MDoubleMatrix m => m.RowCount * m.ColumnCount,
                MDoubleNumber _ => 1,
                MLogical _ => 1,
                MCharArray c => c.Chars.Length,
                _ => throw new System.Exception($"Unknown MObject type {obj.GetType()}"),
            };
        }

        public static bool ToBool(MObject operand)
        {
            return operand switch
            {
                MDoubleNumber { Value: var value } => value != 0.0,
                MLogical { Value: var value } => value,
                MCharArray { Chars: var value } => value.Length > 0,
                MDoubleMatrix m => m.RowCount > 0 && m.ColumnCount > 0,
                _ => throw new System.Exception($"Unknown MObject type {operand.GetType()}"),
            };
        }
    }
}
