using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Services;

public class OrderService : IOrderService
{
	private readonly ApplicationDbContext _context;
	private readonly ICartService _cartService;

	public OrderService(ApplicationDbContext context, CartService cartService)
	{
		_context = context;
		_cartService = cartService;
	}

	public Task<bool> CreateOrder(string orderId, string customerName, string? comment)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteOrder(string orderId)
	{
		throw new NotImplementedException();
	}

	public async Task<List<Order>> GetAllOrders()
	{
		return await _context.Orders.ToListAsync();
	}

	public async Task<Order?> GetOrder(string orderId)
	{
		if (Guid.TryParseExact(orderId, "D", out Guid OrderId))
		{
			return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == OrderId);
		}

		return null;
	}

	public Task<bool> UpdateOrder(string orderId, string? customerName, string? comment)
	{
		throw new NotImplementedException();
	}
}