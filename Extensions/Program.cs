using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new List<string>
            {
                "John",
                "Scott"
            };

            Console.WriteLine(sequence.CountItems());
        }
    }
}
