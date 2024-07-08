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

}