using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Shirt> Shirts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // data seeding
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt() { Id = 1, BrandName = "Nike", Color = "Blue", IsForMen = true, Price = 30.0, Size = 10 },
                new Shirt() { Id = 2, BrandName = "Nike", Color = "Black", IsForMen = true, Price = 35.0, Size = 12 },
                new Shirt() { Id = 3, BrandName = "Adidas", Color = "Pink", IsForMen = false, Price = 28.0, Size = 8 },
                new Shirt() { Id = 4, BrandName = "Adidas", Color = "Yellow", IsForMen = false, Price = 30.0, Size = 9 }
            );
        }
    }
}
