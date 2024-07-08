using System.Data.Common;
using GroceryStoreApi.DTO.Cart;
using GroceryStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Services;

public class TransactionService : ITransactionService
{
	private readonly ApplicationDbContext _context;

	public TransactionService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> AddItem(string cartId, int productId, int quantity = 1)
	{
		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return false;
		}

		var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == CartId);
		if (cart == null)
		{
			return false;
		}

		var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
		if (product == null)
		{
			return false;
		}

		var newTransaction = new Transactions
		{
			CartId = CartId,
			ProductId = productId,
			Quantity = quantity
		};

		var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t =>
			t.CartId == CartId && t.ProductId == productId);

		if (existingTransaction != null)
		{
			newTransaction.Quantity += existingTransaction?.Quantity ?? 1;
		}

		try
		{
			var transaction = await _context.Transactions.AddAsync(newTransaction);
			await _context.SaveChangesAsync();
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error has occurred when adding item to database: {ex.Message}");
		}
	}

	public async Task<bool> RemoveItem(string cartId, int productId, int quantity = 1)
	{
		var transaction = await GetTransaction(cartId, productId);
		if (transaction == null)
		{
			return false;
		}

		try
		{
			if (transaction.Quantity < quantity) return false;

			if (transaction.Quantity == quantity)
			{
				_context.Transactions.Remove(transaction);
			}
			else
			{
				transaction.Quantity -= quantity;
			}

			await _context.SaveChangesAsync();
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception($"Error occurred while updating database: {ex.Message}");
		}

	}

	public async Task<List<CartDTO>?> GetCart(string cartId)
	{
		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return null;
		}

		try
		{
			var cart = await _context.Transactions
				.Where(t => t.CartId == CartId)
				.Select(t => new CartDTO
				{
					productId = t.ProductId,
					quantity = t.Quantity ?? 1
				})
				.ToListAsync();

			if (cart.Count() == 0)
			{
				return null;
			}
			return cart;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error has occurred when accessing Database: {ex.Message}");
		}
	}

	public async Task<Transactions?> GetTransaction(string cartId, int itemId)
	{
		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return null;
		}

		try
		{
			var transaction = await _context.Transactions
			.FirstOrDefaultAsync(t => t.CartId == CartId && t.ProductId == itemId);

			return transaction;
		}
		catch (Exception ex)
		{
			throw new Exception($"Exception has occurred when accessing database: {ex.Message}");
		}
	}

	public async Task<bool> UpdateCart(string cartId, int productId, int newProductId, int quantity = 1)
	{
		try
		{
			var transaction = await GetTransaction(cartId, productId);
			if (transaction == null)
				return false;

			var isRemoved = await RemoveItem(cartId, productId, transaction.Quantity ?? 0);
			if (!isRemoved)
				return false;

			var isAdded = await AddItem(cartId, newProductId, quantity);
			if (!isAdded)
				return false;

			await _context.SaveChangesAsync();
		}
		catch (DbException ex)
		{
			throw new Exception($"Error occurred when updating database: {ex.Message}");
		}

		return true;
	}
}