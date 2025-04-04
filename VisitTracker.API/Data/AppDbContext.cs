using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Models;

namespace VisitTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Visit> Visits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Visit
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Visit>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Visits)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId);

            // Store - Product
            modelBuilder.Entity<Store>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Store)
                .HasForeignKey(p => p.StoreId);

            // Visit - Photo
            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Photos)
                .WithOne(p => p.Visit)
                .HasForeignKey(p => p.VisitId);

            // Product - Photo
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Photos)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
