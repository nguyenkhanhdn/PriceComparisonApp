using Microsoft.EntityFrameworkCore;
using PriceComparisonApp.Models;

namespace PriceComparisonApp.Data
{
    public class PriceComparisonDbContext:DbContext
    {
        public PriceComparisonDbContext(DbContextOptions<PriceComparisonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
