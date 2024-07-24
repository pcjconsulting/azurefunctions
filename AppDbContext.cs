using Microsoft.EntityFrameworkCore;

namespace azurefunctions
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options) 
        { 
        }

        public DbSet<Product> Products { get; set; }

    }
}
