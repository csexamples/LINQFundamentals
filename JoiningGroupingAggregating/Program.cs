using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JoiningGroupingAggregating
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query = from car in cars
                        join manufacturer in manufacturers
                        on new { car.Manufacturer, car.Year } equals new { Manufacturer = manufacturer.Name, manufacturer.Year }
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquaters,
                            car.Name,
                            car.Combined
                        };

            var query2 = cars.Join(manufacturers,
                              c => new { c.Manufacturer, c.Year },
                              m => new { Manufacturer = m.Name, m.Year }, 
                              (c, m) => new
                              {
                                  m.Headquaters,
                                  c.Name,
                                  c.Combined
                              })
                             .OrderByDescending(c => c.Combined)
                             .ThenBy(c => c.Name);

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Headquaters} {car.Name} : {car.Combined}");
            }

            var queryOfGroups = from car in cars
                                group car by car.Manufacturer.ToUpper() into manufacturer
                                orderby manufacturer.Key
                                select manufacturer;

            var queryOfGroups2 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                                     .OrderBy(g => g.Key);

            foreach (var group in queryOfGroups2)
            {
                Console.WriteLine(group.Key);

                foreach(var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            var queryOfGroupJoin = from manufacturer in manufacturers
                                   join car in cars
                                   on manufacturer.Name equals car.Manufacturer into carGroup
                                   orderby manufacturer.Name
                                   select new
                                   {
                                       Manufacturer = manufacturer,
                                       Cars = carGroup
                                   };

            var queryOfGroupJoin2 = manufacturers.GroupJoin(cars,
                                                            m => m.Name,
                                                            c => c.Manufacturer,
                                                            (m, g) => new
                                                            {
                                                                Manufacturer = m,
                                                                Cars = g
                                                            })
                                                            .OrderBy(g => g.Manufacturer.Name);

            foreach (var group in queryOfGroupJoin2)
            {
                Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquaters}");

                foreach(var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            var queryOfGroupJoin3 = from manufacturer in manufacturers
                                    join car in cars
                                    on manufacturer.Name equals car.Manufacturer into carGroup
                                    orderby manufacturer.Name
                                    select new
                                    {
                                        Manufacturer = manufacturer,
                                        Cars = carGroup
                                    }
                                    into result
                                    group result by result.Manufacturer.Headquaters;


            var queryOfGroupJoin4 = manufacturers.GroupJoin(cars,
                                                            m => m.Name,
                                                            c => c.Manufacturer,
                                                           (m, g) => new
                                                           {
                                                               Manufacturer = m,
                                                               Cars = g
                                                           })
                                                           .GroupBy(g => g.Manufacturer.Headquaters);

            foreach (var group in queryOfGroupJoin4)
            {
                Console.WriteLine(group.Key);

                foreach (var car in group.SelectMany(g => g.Cars)
                                         .OrderByDescending(c => c.Combined)
                                         .Take(3))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            var aggregationQuery = from car in cars
                                   group car by car.Manufacturer into carGroup
                                   select new
                                   {
                                       Name = carGroup.Key,
                                       Max = carGroup.Max(c => c.Combined),
                                       Min = carGroup.Min(c => c.Combined),
                                       Avg = carGroup.Average(c => c.Combined)
                                   }
                                   into result
                                   orderby result.Max descending
                                   select result;

            var aggregationQuery2 = cars.GroupBy(c => c.Manufacturer)
                                        .Select(g =>
                                        {
                                            var result = g.Aggregate(new CarStatistics(),
                                                                     (acc, c) => acc.Accumulate(c),
                                                                     acc => acc.Compute());

                                            return new
                                            {
                                                Name = g.Key,
                                                result.Max,
                                                result.Min,
                                                result.Avg
                                            };

                                        })
                                        .OrderByDescending(r => r.Max);

            foreach (var result in aggregationQuery2)
            {
                Console.WriteLine(result.Name);
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Avg}");
            }
        }

        public static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                       .Skip(1)
                       .Where(line => line.Length > 0)
                       .ToCar();

            return query.ToList();
        }

        public static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Where(l => l.Length > 0)
                            .Select(l =>
                            {
                                var columns = l.Split(',');

                                return new Manufacturer
                                {
                                    Name = columns[0],
                                    Headquaters = columns[1],
                                    Year = int.Parse(columns[2])
                                };
                            });

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

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = int.MinValue;
            Min = int.MaxValue;
        }

        public int Max { get; set; }

        public int Min { get; set; }

        public double Avg { get; set; }

        public int Total { get; set; }

        public int Count { get; set; }

        public CarStatistics Accumulate(Car c)
        {
            Max = Math.Max(Max, c.Combined);
            Min = Math.Min(Min, c.Combined);
            Total += c.Combined;
            Count += 1;

            return this;
        }

        public CarStatistics Compute()
        {
            Avg = Total / Count;

            return this;
        }
    }
}
