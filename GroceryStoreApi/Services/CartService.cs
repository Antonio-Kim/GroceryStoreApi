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
		var newCart = new Cart { CartId = newCartId };
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
}