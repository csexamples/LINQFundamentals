using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilteringOrderingProjecting
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .Select(c => new
                            {
                                c.Name,
                                c.Combined
                            })
                            .Take(10);

            foreach (var car in query)
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }

            var top = cars.OrderByDescending(c => c.Combined)
                          .FirstOrDefault(c => c.Manufacturer == "BMW" && c.Year == 2016);

            Console.WriteLine($"Top BMW: {top.Name} : {top.Combined}");

            var result = cars.Any(c => c.Manufacturer == "Ford");

            if (result) Console.WriteLine("The list contains some cars of manufacturer Ford");

            var characters = cars.SelectMany(c => c.Name)
                                 .OrderBy(c => c);
        }

        public static List<Car> ProcessFile(string path)
        {
            var query = File.ReadAllLines(path)
                       .Skip(1)
                       .Where(line => line.Length > 0)
                       .ToCar();

            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
