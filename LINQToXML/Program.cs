using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FilteringOrderingProjecting;

namespace LINQToXML
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        public static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var ns = (XNamespace)"cars/2016";
            var ex = (XNamespace)"cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars", from record in records
                                                 select new XElement(ex + "Car", new XAttribute("Name", record.Name),
                                                                                 new XAttribute("Combined", record.Combined),
                                                                                 new XAttribute("Manufacturer", record.Manufacturer)));

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            document.Add(cars);
            document.Save("fuel.xml");
        }

        public static void QueryXml()
        {
            var ns = (XNamespace)"cars/2016";
            var ex = (XNamespace)"cars/2016/ex";
            var document = XDocument.Load("fuel.xml");

            var query = from element in document.Element(ns + "Cars").Elements(ex + "Car")
                        where element.Attribute("Manufacturer").Value == "BMW"
                        select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
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
    }
}
