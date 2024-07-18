using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 4643,
            Category = "coffee",
            Name = "Starbucks Coffee Variety Pack, 100% Arabica",
            Manufacturer = "Starbucks",
            Price = 40.91M,
            CurrentStock = 14,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 4646,
            Category = "coffee",
            Name = "Ethical Bean Medium Dark Roast",
            Manufacturer = "Ethical Bean",
            Price = 7.78M,
            CurrentStock = 10,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 4641,
            Category = "coffee",
            Name = "Don Francisco Colombia Supremo Medium Roast",
            Manufacturer = "Don Francisco",
            Price = 9.76M,
            CurrentStock = 15,
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 1225,
            Category = "fresh-produce",
            Name = "1/2 in. Brushless Hammer Drill",
            Manufacturer = "Bosch",
            Price = 12.98M,
            CurrentStock = 12,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 3674,
            Category = "fresh-produce",
            Name = "20V Max Cordless Drill Combo Kit",
            Manufacturer = "DEWALT",
            Price = 10.96M,
            CurrentStock = 3,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 2585,
            Category = "fresh-produce",
            Name = "Green Cabbage Organic",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 3.02M,
            CurrentStock = 30,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 5851,
            Category = "fresh-produce",
            Name = "Cucumber Organic",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 0.95M,
            CurrentStock = 0,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 8739,
            Category = "fresh-produce",
            Name = "Fresh Spinach Organic",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 2.95M,
            CurrentStock = 27,
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 2177,
            Category = "meat-seafood",
            Name = "Cosco Three Step Steel Platform",
            Manufacturer = "CoscoProducts",
            Price = 2.95M,
            CurrentStock = 9,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 1709,
            Category = "meat-seafood",
            Name = "Beef Choice Angus Ribeye Steak",
            Manufacturer = "Taylor & Partners Organic Meats",
            Price = 14.95M,
            CurrentStock = 17,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 7395,
            Category = "meat-seafood",
            Name = "Boneless Skinless Chicken Breasts",
            Manufacturer = "Taylor & Partners Organic Meats",
            Price = 7.45M,
            CurrentStock = 12,
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 8554,
            Category = "candy",
            Name = "Kinder Joy Eggs",
            Manufacturer = "Ferrero",
            Price = 1.05M,
            CurrentStock = 3,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 6483,
            Category = "candy",
            Name = "Cadbury Milk Chocolate",
            Manufacturer = "Cadbury",
            Price = 2.65M,
            CurrentStock = 2,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 5774,
            Category = "candy",
            Name = "HERSHEY'S, Milk Chocolate Almond",
            Manufacturer = "Hershey's",
            Price = 1.45M,
            CurrentStock = 1,
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 8753,
            Category = "dairy",
            Name = "Reduced Fat Milk",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 3.45M,
            CurrentStock = 24,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 9482,
            Category = "dairy",
            Name = "Whole Milk",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 3.55M,
            CurrentStock = 16,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 5477,
            Category = "dairy",
            Name = "Cream Cheese",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 2.95M,
            CurrentStock = 12,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 5478,
            Category = "dairy",
            Name = "Low Fat Vanilla Yogurt",
            Manufacturer = "Jack&Mary Organic Farms",
            Price = 1.95M,
            CurrentStock = 16,
        });
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 4875,
            Category = "bread-bakery",
            Name = "2800 Watt Inverter Generator",
            Manufacturer = "Honda",
            Price = 47.45M,
            CurrentStock = 10,
        });

        modelBuilder.Entity<Transactions>()
            .HasKey(t => new { t.ProductId, t.CartId });

        modelBuilder.Entity<Transactions>()
            .HasOne(t => t.Product)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transactions>()
            .HasOne(t => t.Cart)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CartId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    public virtual DbSet<Product> Products => Set<Product>();
    public virtual DbSet<Cart> Carts => Set<Cart>();
    public virtual DbSet<Transactions> Transactions => Set<Transactions>();
    public virtual DbSet<Order> Orders => Set<Order>();
}