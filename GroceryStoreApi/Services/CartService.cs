using System.Data.Common;
using GroceryStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartAsync(string cartId)
    {
        if (Guid.TryParseExact(cartId, "D", out Guid CartId))
        {
            return await _context.Carts.FindAsync(CartId);
        }
        else
        {
            return null;
        }
    }

    public async Task<string> NewCartAsync()
    {
        Guid newCartId = Guid.NewGuid();
        DateTime created = DateTime.Now;
        var newCart = new Cart
        {
            CartId = newCartId,
            Created = created
        };
        try
        {
            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();
            return newCartId.ToString();
        }
        catch (DbException ex)
        {
            throw new Exception($"Error occured when saving to database: {ex.Message}");
        }
    }

    public async Task<bool> DeleteCartAsync(string cartId)
    {
        if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
        {
            return false;
        }

        try
        {
            var cart = await GetCartAsync(cartId);
            if (cart == null)
            {
                return false;
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbException ex)
        {
            throw new Exception($"Error occurred when deleting cart from database: {ex.Message}");
        }
    }
}