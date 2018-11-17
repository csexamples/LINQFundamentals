using Microsoft.EntityFrameworkCore;

namespace LinqToEfCore
{
    public class CarDb : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433; Database=CarDb; User=SA; Password=password111!");
        }
    }
}
