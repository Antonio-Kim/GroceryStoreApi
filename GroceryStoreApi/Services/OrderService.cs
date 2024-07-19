using System.Data.Common;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ICartService _cartService;

    public OrderService(ApplicationDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    public async Task<string> CreateOrder(string cartId, string customerName, string? comment = "")
    {
        if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
        {
            return null;
        }

        var cart = await _cartService.GetCartAsync(cartId);
        if (cart == null) return null;

        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            CartId = CartId,
            CustomerName = customerName,
            Comment = comment ?? string.Empty
        };

        try
        {
            await _context.Orders.AddAsync(order);
            await _cartService.DeleteCartAsync(cartId);
            await _context.SaveChangesAsync();
            return order.OrderId.ToString();
        }
        catch (DbException ex)
        {
            throw new Exception($"Error occurred when adding order: {ex.Message}");
        }
    }

    public async Task<bool> DeleteOrder(string orderId)
    {
        if (!Guid.TryParseExact(orderId, "D", out Guid OrderId)) return false;

        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == OrderId);
            if (order == null)
            {
                return false;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbException ex)
        {
            throw new Exception($"Error when deleting order on database: {ex.Message}");
        }
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

    public async Task<bool> UpdateOrder(string orderId, string? customerName, string? comment)
    {
        if (!Guid.TryParseExact(orderId, "D", out Guid OrderId))
        {
            return false;
        }

        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == OrderId);
            if (order == null) return false;
            var updatedOrder = new Order
            {
                OrderId = OrderId,
                CartId = order.CartId,
                CustomerName = customerName ?? order.CustomerName,
                Comment = comment ?? order.Comment,
            };
            _context.Orders.Remove(order);
            _context.Orders.Add(updatedOrder);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbException ex)
        {
            throw new Exception($"Error occurred when updating order: {ex.Message}");
        }
    }
}