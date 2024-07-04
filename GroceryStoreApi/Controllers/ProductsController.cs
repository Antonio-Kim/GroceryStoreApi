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

	[HttpGet(Name = "GetProducts")]
	public async Task<RestDTO<ProductsDTO[]>> GetAllProducts()
	{
		var query = _context.Products
			.Select(p => new ProductsDTO
			{
				Id = p.Id,
				Category = p.Category,
				Name = p.Name,
				InStock = p.CurrentStock != 0
			});

		return new RestDTO<ProductsDTO[]>
		{
			Data = await query.ToArrayAsync(),
			Links = new List<LinkDTO>
			{
				new LinkDTO
				(
					Url.Action(null, "Products", null, Request.Scheme)!,
					"self",
					"GET"
				)
			}
		};
	}

	[HttpGet("{id}", Name = "GetProductById")]
	public async Task<ActionResult<RestDTO<Product>>> GetProductById(int id)
	{
		var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

		if (product == null)
		{
			return NotFound(new { error = $"No product with id {id}." });
		}

		var response = new RestDTO<Product>()
		{
			Data = product,
			Links = new List<LinkDTO>()
			{
				new LinkDTO
				(
					Url.Action(null, "Products", null, Request.Scheme)!,
					"self",
					"GET"
				)
			}
		};

		return Ok(response);
	}
}