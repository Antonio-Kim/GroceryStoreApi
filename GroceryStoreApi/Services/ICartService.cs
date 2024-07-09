using GroceryStoreApi.Models;

namespace GroceryStoreApi.Services;

public interface ICartService
{
	Task<Cart?> GetCartAsync(string cartId);
	Task<string> NewCartAsync();
	Task<bool> DeleteCartAsync(string cartId);
}