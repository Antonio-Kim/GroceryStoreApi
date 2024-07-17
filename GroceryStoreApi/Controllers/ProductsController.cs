using GroceryStoreApi.DTO;
using GroceryStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public class ProductsController : ControllerBase
{
	private readonly ApplicationDbContext _context;


	public ProductsController(ApplicationDbContext context)
	{
		_context = context;
	}

	[HttpGet(Name = "Get All Products")]
	public async Task<IActionResult> GetProducts(
		string? category = null,
		int results = 20,
		bool? available = null)
	{
		var query = _context.Products
			.AsQueryable();

		if (!string.IsNullOrEmpty(category))
		{
			query = query.Where(p => p.Category == category);
		}

		if (available.HasValue)
		{
			query = query.Where(p => p.CurrentStock > 0 == available.Value);
		}
		var products = await query
						.Take(results)
						.Select(p => new
						{
							p.Id,
							p.Category,
							p.Name,
							InStock = p.CurrentStock > 0
						})
						.ToListAsync();

		return Ok(products);
	}

	[HttpGet("{id}", Name = "GetProductById")]
	[Route("/{id}")]
	public async Task<ActionResult<object>> GetProductById(int id)
	{
		var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

		if (product == null)
		{
			return NotFound(new { error = $"No product with id {id}." });
		}

		var links = new List<LinkDTO>()
		{
			new LinkDTO(
				Url.Action("GetProductById", "products", null, Request.Scheme)!,
				"self",
				"GET"
			),
		};

		return new
		{
			Data = product,
			Links = links
		};
	}
}