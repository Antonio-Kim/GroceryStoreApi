using GroceryStoreApi.Models;

namespace GroceryStoreApi.Services;

public interface IOrderService
{
    public Task<List<Order>> GetAllOrders();
    public Task<Order?> GetOrder(string orderId);
    public Task<string> CreateOrder(string orderId, string customerName, string? comment);
    public Task<bool> UpdateOrder(string orderId, string? customerName, string? comment);
    public Task<bool> DeleteOrder(string orderId);
}