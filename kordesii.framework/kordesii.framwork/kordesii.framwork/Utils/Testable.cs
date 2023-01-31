using System;
using System.Collections.Generic;
using System.Text;

namespace kordesii.framwork.Utils
{
    public static class Testable
    {
        public static bool GetRandomBool(Random seed)
        {
            return seed.Next(2) == 1;
        }

    }
}
