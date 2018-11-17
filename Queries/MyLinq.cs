using System;
using System.Collections.Generic;

namespace QueriesWithDeferredExecution

{
    public static class MyLinq
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            foreach (var item in sequence)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<double> Random()
        {
            var random = new Random();

            while (true)
            {
                yield return random.NextDouble();
            }
        }
    }
}
