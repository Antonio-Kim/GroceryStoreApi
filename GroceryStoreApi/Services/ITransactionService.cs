using GroceryStoreApi.DTO.Cart;
using GroceryStoreApi.Models;

namespace GroceryStoreApi.Services;

public interface ITransactionService
{
    Task<List<CartDTO>?> GetCart(string cartId);
    Task<Transactions?> GetTransaction(string cartId, int itemId);
    Task<bool> AddItem(string cartId, int productId, int quantity = 1);
    Task<bool> UpdateCart(string cartId, int productId, int newProductId, int quantity);
    Task<bool> RemoveItem(string cartId, int productId, int quantity = 1);
}