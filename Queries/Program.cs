using System;
using System.Collections.Generic;
using System.Linq;

namespace QueriesWithDeferredExecution
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight", Rating = 8.9f, Year = 2008 },
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Casablanca", Rating = 8.5f, Year = 1942 },
                new Movie { Title = "Star Wars V", Rating = 8.7f, Year = 1980 }
            };

            // deferred streaming execution
            var query = movies.Filter(m => m.Year > 2000);

            foreach (var movie in query)
            {
                Console.WriteLine(movie.Title);
            }

            var query2 = MyLinq.Random().Where(n => n > 0.5).Take(10);

            foreach (var number in query2)
            {
                Console.WriteLine(number);
            }

            // deferred non-streaming execution
            // OrderByDescending has to look through all of the filtered items to determine which to display first
            // it happens once when the first query is executed
            var query3 = movies.Where(m => m.Year > 2000).OrderByDescending(m => m.Rating);

            // immediate execution
            var movieList = movies.Filter(m => m.Year > 2000).ToList();
        }
    }
}
