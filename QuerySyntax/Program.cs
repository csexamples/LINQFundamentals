using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySyntax
{
    class Program
    {
        static void Main(string[] args)
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John" },
                new Employee { Id = 2, Name = "Scott" },
                new Employee { Id = 3, Name = "Alex" },
                new Employee { Id = 4, Name = "Susan" }
            };

            var query = from employee in employees
                        where employee.Name.StartsWith("S")
                        orderby employee.Name
                        select employee;

            foreach (var employee in query)
            {
                Console.WriteLine(employee.Name);
            }
        }
    }
}
