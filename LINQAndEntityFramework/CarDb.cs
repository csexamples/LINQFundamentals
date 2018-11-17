using Microsoft.EntityFrameworkCore;

namespace LINQAndEntityFramework
{
    public class CarDb : DbContext
    {
        public DbSet<Car> Cars { get; set; }
    }
}
