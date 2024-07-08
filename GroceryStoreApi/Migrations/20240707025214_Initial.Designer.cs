﻿// <auto-generated />
using System;
using GroceryStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GroceryStoreApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240707025214_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("GroceryStoreApi.Models.Cart", b =>
                {
                    b.Property<Guid>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("CartId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("GroceryStoreApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentStock")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "current_stock");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasPrecision(6, 2)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 4643,
                            Category = "coffee",
                            CurrentStock = 14,
                            Manufacturer = "Starbucks",
                            Name = "Starbucks Coffee Variety Pack, 100% Arabica",
                            Price = 40.91m
                        },
                        new
                        {
                            Id = 4646,
                            Category = "coffee",
                            CurrentStock = 10,
                            Manufacturer = "Ethical Bean",
                            Name = "Ethical Bean Medium Dark Roast",
                            Price = 7.78m
                        },
                        new
                        {
                            Id = 4641,
                            Category = "coffee",
                            CurrentStock = 15,
                            Manufacturer = "Don Francisco",
                            Name = "Don Francisco Colombia Supremo Medium Roast",
                            Price = 9.76m
                        },
                        new
                        {
                            Id = 1225,
                            Category = "fresh-produce",
                            CurrentStock = 12,
                            Manufacturer = "Bosch",
                            Name = "1/2 in. Brushless Hammer Drill",
                            Price = 12.98m
                        },
                        new
                        {
                            Id = 3674,
                            Category = "fresh-produce",
                            CurrentStock = 3,
                            Manufacturer = "DEWALT",
                            Name = "20V Max Cordless Drill Combo Kit",
                            Price = 10.96m
                        },
                        new
                        {
                            Id = 2585,
                            Category = "fresh-produce",
                            CurrentStock = 30,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Green Cabbage Organic",
                            Price = 3.02m
                        },
                        new
                        {
                            Id = 5851,
                            Category = "fresh-produce",
                            CurrentStock = 0,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Cucumber Organic",
                            Price = 0.95m
                        },
                        new
                        {
                            Id = 8739,
                            Category = "fresh-produce",
                            CurrentStock = 27,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Fresh Spinach Organic",
                            Price = 2.95m
                        },
                        new
                        {
                            Id = 2177,
                            Category = "meat-seafood",
                            CurrentStock = 9,
                            Manufacturer = "CoscoProducts",
                            Name = "Cosco Three Step Steel Platform",
                            Price = 2.95m
                        },
                        new
                        {
                            Id = 1709,
                            Category = "meat-seafood",
                            CurrentStock = 17,
                            Manufacturer = "Taylor & Partners Organic Meats",
                            Name = "Beef Choice Angus Ribeye Steak",
                            Price = 14.95m
                        },
                        new
                        {
                            Id = 7395,
                            Category = "meat-seafood",
                            CurrentStock = 12,
                            Manufacturer = "Taylor & Partners Organic Meats",
                            Name = "Boneless Skinless Chicken Breasts",
                            Price = 7.45m
                        },
                        new
                        {
                            Id = 8554,
                            Category = "candy",
                            CurrentStock = 3,
                            Manufacturer = "Ferrero",
                            Name = "Kinder Joy Eggs",
                            Price = 1.05m
                        },
                        new
                        {
                            Id = 6483,
                            Category = "candy",
                            CurrentStock = 2,
                            Manufacturer = "Cadbury",
                            Name = "Cadbury Milk Chocolate",
                            Price = 2.65m
                        },
                        new
                        {
                            Id = 5774,
                            Category = "candy",
                            CurrentStock = 1,
                            Manufacturer = "Hershey's",
                            Name = "HERSHEY'S, Milk Chocolate Almond",
                            Price = 1.45m
                        },
                        new
                        {
                            Id = 8753,
                            Category = "dairy",
                            CurrentStock = 24,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Reduced Fat Milk",
                            Price = 3.45m
                        },
                        new
                        {
                            Id = 9482,
                            Category = "dairy",
                            CurrentStock = 16,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Whole Milk",
                            Price = 3.55m
                        },
                        new
                        {
                            Id = 5477,
                            Category = "dairy",
                            CurrentStock = 12,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Cream Cheese",
                            Price = 2.95m
                        },
                        new
                        {
                            Id = 5478,
                            Category = "dairy",
                            CurrentStock = 16,
                            Manufacturer = "Jack&Mary Organic Farms",
                            Name = "Low Fat Vanilla Yogurt",
                            Price = 1.95m
                        },
                        new
                        {
                            Id = 4875,
                            Category = "bread-bakery",
                            CurrentStock = 10,
                            Manufacturer = "Honda",
                            Name = "2800 Watt Inverter Generator",
                            Price = 47.45m
                        });
                });

            modelBuilder.Entity("GroceryStoreApi.Models.Transactions", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<Guid>("CartId")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(0);

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId", "CartId");

                    b.HasIndex("CartId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("GroceryStoreApi.Models.Transactions", b =>
                {
                    b.HasOne("GroceryStoreApi.Models.Cart", "Cart")
                        .WithMany("Transactions")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GroceryStoreApi.Models.Product", "Product")
                        .WithMany("Transactions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GroceryStoreApi.Models.Cart", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("GroceryStoreApi.Models.Product", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
