using System.ComponentModel.DataAnnotations;
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
		[RegularExpression("coffee|fresh-produce|meat-seafood|dairy|candy|bread-bakery",
		ErrorMessage = "Invalid value for query parameter 'category'. Must be one of: meat-seafood, fresh-produce, candy, bread-bakery, dairy, eggs, coffee")]
		string? category = null,
		[Range(1, 20)] int results = 20,
		bool? available = null)
	{
		if (!ModelState.IsValid)
		{
			var errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			var errorResponse = new
			{
				error = errors.FirstOrDefault()
			};

			return BadRequest(errorResponse);
		}
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
	public async Task<IActionResult> GetProductById(int id)
	{
		var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

		if (product == null)
		{
			return NotFound(new { error = $"No product with id {id}." });
		}

		var response = new
		{
			product.Id,
			product.Name,
			product.Category,
			product.Manufacturer,
			product.Price,
			product.CurrentStock,
			InStock = product.CurrentStock > 0
		};

		return Ok(response);
	}
}