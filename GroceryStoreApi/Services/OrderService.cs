using System.Data.Common;
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

	public async Task<bool> CreateOrder(string cartId, string customerName, string? comment = "")
	{
		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return false;
		}

		var cart = await _cartService.GetCartAsync(cartId);
		if (cart == null) return false;

		var order = new Order
		{
			OrderId = Guid.NewGuid(),
			CartId = CartId,
			CustomerName = customerName,
			Comment = comment
		};

		try
		{
			await _context.Orders.AddAsync(order);
			await _cartService.DeleteCartAsync(cartId);
			await _context.SaveChangesAsync();
			return true;
		}
		catch (DbException ex)
		{
			throw new Exception($"Error occurred when adding order: {ex.Message}");
		}
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