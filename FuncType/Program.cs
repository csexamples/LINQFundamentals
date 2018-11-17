using System;

namespace FuncType
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int, int> add = (x, y) => x + y;

            Func<int, int> square = x => x * x;

            Action<int> write = x => Console.WriteLine(x);

            write(square(add(3, 5)));
        }
    }
}
