using Microsoft.EntityFrameworkCore;

namespace WorkshopAspNetCore
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Person> People { get; set; }
    }
}