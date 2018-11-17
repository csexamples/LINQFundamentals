using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace LINQAndEntityFramework
{
    public class Startupp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarDb>(options =>
                options.UseSqlServer(DbConfiguration.GetConnectionString("BloggingDatabase")));
        }
    }
}
