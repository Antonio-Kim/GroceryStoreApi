using GroceryStoreApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GroceryStoreTests.Fakes;

public class ApplicationDbContextFakeBuilder : IDisposable
{
    private readonly ApplicationDbContextFake _context = new();
    private EntityEntry<Product>? _starbucksCoffee;
    private EntityEntry<Product>? _cabbage;
    private EntityEntry<Product>? _cucumber;
    private EntityEntry<Product>? _milk;
    private EntityEntry<Product>? _candy;

    private EntityEntry<Cart>? _cartOne;
    private EntityEntry<Cart>? _cartTwo;
    private EntityEntry<Cart>? _cartThree;

    private EntityEntry<Transactions>? _transactionOne;
    private EntityEntry<Transactions>? _transactionTwo;
    private EntityEntry<Transactions>? _transactionThree;
    private EntityEntry<Transactions>? _transactionFour;
    private EntityEntry<Transactions>? _transactionFive;

    private EntityEntry<Order>? _orderOne;
    private EntityEntry<Order>? _orderTwo;


    public ApplicationDbContextFake Build()
    {
        _context.SaveChangesAsync();
        return _context;
    }

    public ApplicationDbContextFakeBuilder WithOneProduct()
    {
        _starbucksCoffee = _context.Products.Add(
            new Product
            {
                Id = 4643,
                Category = "coffee",
                Name = "Starbucks Coffee Variety Pack, 100% Arabica",
                Manufacturer = "Starbucks",
                Price = 40.91M,
                CurrentStock = 14,
            });
        return this;
    }

    public ApplicationDbContextFakeBuilder WithProducts()
    {
        _starbucksCoffee = _context.Products.Add(
            new Product
            {
                Id = 4646,
                Category = "coffee",
                Name = "Starbucks Coffee Variety Pack, 100% Arabica",
                Manufacturer = "Starbucks",
                Price = 40.91M,
                CurrentStock = 14,
            });

        _cabbage = _context.Products.Add(
            new Product
            {
                Id = 2585,
                Category = "fresh-produce",
                Name = "Green Cabbage Organic",
                Manufacturer = "Jack&Mary Organic Farms",
                Price = 3.02M,
                CurrentStock = 30,
            }
        );

        _cucumber = _context.Products.Add(
            new Product
            {
                Id = 5851,
                Category = "fresh-produce",
                Name = "Cucumber Organic",
                Manufacturer = "Jack&Mary Organic Farms",
                Price = 0.95M,
                CurrentStock = 0,
            }
        );

        _milk = _context.Products.Add(
            new Product
            {
                Id = 9482,
                Category = "dairy",
                Name = "Whole Milk",
                Manufacturer = "Jack&Mary Organic Farms",
                Price = 3.55M,
                CurrentStock = 16,
            });

        _candy = _context.Products.Add(
            new Product
            {
                Id = 8554,
                Category = "candy",
                Name = "Kinder Joy Eggs",
                Manufacturer = "Ferrero",
                Price = 1.05M,
                CurrentStock = 3,
            }
        );

        return this;
    }

    public ApplicationDbContextFakeBuilder WithOneCart()
    {
        _cartOne = _context.Carts.Add(
            new Cart
            {
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                Created = DateTime.Now
            }
        );
        return this;
    }

    public ApplicationDbContextFakeBuilder WithCarts()
    {
        _cartOne = _context.Carts.Add(
            new Cart
            {
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                Created = DateTime.Now
            }
        );
        _cartTwo = _context.Carts.Add(
            new Cart
            {
                CartId = Guid.Parse("2E892988-18F1-4DA7-2252-1FB697891A58"),
                Created = DateTime.Now
            }
        );
        _cartThree = _context.Carts.Add(
            new Cart
            {
                CartId = Guid.Parse("1C892986-18F1-82AD-2252-1FB6978922FA"),
                Created = DateTime.Now
            }
        );
        return this;
    }

    public ApplicationDbContextFakeBuilder WithTransactions()
    {
        _transactionOne = _context.Transactions.Add(
            new Transactions
            {
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                ProductId = 4646,
                Quantity = 1
            }
        );
        _transactionTwo = _context.Transactions.Add(
            new Transactions
            {
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                ProductId = 2585,
                Quantity = 4
            }
        );
        _transactionThree = _context.Transactions.Add(
            new Transactions
            {
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                ProductId = 3674,
                Quantity = 8
            }
        );
        _transactionFour = _context.Transactions.Add(
            new Transactions
            {
                CartId = Guid.Parse("1C892986-18F1-82AD-2252-1FB6978922FA"),
                ProductId = 5851,
                Quantity = 5,
            }
        );
        _transactionFive = _context.Transactions.Add(
            new Transactions
            {
                CartId = Guid.Parse("2E892988-18F1-4DA7-2252-1FB697891A58"),
                ProductId = 1709,
                Quantity = 3,
            }
        );

        return this;
    }

    public ApplicationDbContextFakeBuilder WithOrders()
    {
        _orderOne = _context.Orders.Add(
            new Order
            {
                OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
                CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
                CustomerName = "John Doe",
                Comment = ""
            }
        );
        _orderTwo = _context.Orders.Add(
            new Order
            {
                OrderId = Guid.Parse("5A683325-73DF-882A-351E-2E924AE8EC3F"),
                CartId = Guid.Parse("2E892988-18F1-4DA7-2252-1FB697891A58"),
                CustomerName = "Jane Doe",
                Comment = "Express Delivery"
            }
        );

        return this;
    }
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}