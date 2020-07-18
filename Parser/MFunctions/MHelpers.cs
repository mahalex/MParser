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
    }
}
