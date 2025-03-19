using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using PriceComparisonApp.Models;
using System.Configuration;

namespace PriceComparisonApp.Data
{
    public class PriceComparisonDbContext:DbContext
    {
    
        public PriceComparisonDbContext(DbContextOptions<PriceComparisonDbContext> options)
            : base(options)
        {
        }


        public DbSet<PriceComparisonApp.Models.Product> Products { get; set; }
    }
}
