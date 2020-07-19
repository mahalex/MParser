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

        public static bool ToBool(MObject operand)
        {
            return operand switch
            {
                MDoubleNumber { Value: var value } => value != 0.0,
                MLogical { Value: var value } => value,
                MCharArray { Chars: var value } => value.Length > 0,
                _ => throw new System.Exception($"Unknown MObject type {operand.GetType()}"),
            };
        }
    }
}
