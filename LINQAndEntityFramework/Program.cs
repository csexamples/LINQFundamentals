using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace LINQAndEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());

            InsertData();
            QueryData();
        }

        public static void QueryData()
        {
            var db = new CarDb();

            var query = db.Cars.GroupBy(c => c.Manufacturer)
                               .Select(g => new
                               {
                                   Name = g.Key,
                                   Cars = g.OrderByDescending(c => c.Combined).Take(2)
                               });

            foreach (var group in query)
            {
                Console.WriteLine(group.Name);

                foreach (var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        public static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }

                db.SaveChanges();
            }
        }

        public static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                       .Skip(1)
                       .Where(line => line.Length > 0)
                       .Select(l =>
                       {
                           var columns = l.Split(',');

                           return new Car
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
                       });

            return query.ToList();
        }
    }
}
