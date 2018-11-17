using System;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class MyLinq
    {
        public static int CountItems<T>(this IEnumerable<T> sequence)
        {
            int count = 0;

            foreach(var item in sequence)
            {
                count++;
            }

            return count;
        }
    }
}
