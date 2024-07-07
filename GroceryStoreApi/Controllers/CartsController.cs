using System.ComponentModel.DataAnnotations;
using GroceryStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GroceryStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(NoStore = true)]
public class CartsController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public CartsController(ApplicationDbContext context)
	{
		_context = context;
	}

	[HttpPost(Name = "Create Cart")]
	public async Task<IActionResult> Post()
	{
		var newCart = new Cart
		{
			CartId = Guid.NewGuid()
		};
		try
		{
			_context.Carts.Add(newCart);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(Post), new { created = true, cartId = newCart.CartId });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { error = $"Internal Error: {ex.Message}" });
		}
	}

	[HttpGet("{cartId}", Name = "Get Cart")]
	public async Task<IActionResult> GetCart(string cartId)
	{
		var id = Guid.ParseExact(cartId, "D");
		var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == id);

		if (cart == null)
		{
			return NotFound($"No cart with {cartId} exists.");
		}

		return Ok();
	}

	[HttpPost("{cartId}/items", Name = "Add Item to cart")]
	public async Task<IActionResult> PostItemToCart(string cartId, [FromBody] CartAddItemDTO input)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return BadRequest("Invalid cartId format.");
		}

		var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == CartId);
		if (cart == null)
		{
			return NotFound($"No cart with {cartId} exists.");
		}

		var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == input.productId);
		if (product == null)
		{
			return NotFound($"No product with {input.productId} exists.");
		}

		if (input.quantity < 1)
		{
			return BadRequest("Quantity must be greater than one");
		}

		var newTransaction = new Transactions()
		{
			CartId = CartId,
			ProductId = input.productId,
			Quantity = input.quantity
		};

		_context.Transactions.Add(newTransaction);
		await _context.SaveChangesAsync();

		return Created();
	}

	[HttpGet("{cartId}/items", Name = "GetItemsInCart")]
	public async Task<IActionResult> GetItemsInCart(string cartId)
	{
		if (!Guid.TryParseExact(cartId, "D", out Guid CartId))
		{
			return BadRequest("Invalid cartId format.");
		}
		var cart = await _context.Transactions.FirstOrDefaultAsync(t => t.CartId == CartId);
		if (cart == null)
		{
			return NotFound($"Cart ID {cartId} has done no actions.");
		}

		var productsInCart = await _context.Transactions
			.Where(t => t.CartId == CartId)
			.Select(t => new
			{
				t.Product.Id,
				t.Product.Name,
			})
			.ToListAsync();

		return Ok(productsInCart);
	}
}