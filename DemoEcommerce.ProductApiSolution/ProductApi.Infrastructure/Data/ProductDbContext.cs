using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
       
        //cofiguration of the model can be done here using fluent API if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Automatic registration - finds ALL IEntityTypeConfiguration<T> classes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        }

        // Define DbSets for your entities here
        public DbSet<Product> Products { get; set; }
    }
}
