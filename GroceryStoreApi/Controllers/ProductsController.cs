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
	public async Task<RestDTO<ProductsDTO[]>> GetAllProducts([FromQuery] ProductRequestDTO input)
	{
		var query = _context.Products.AsQueryable();

		if (!string.IsNullOrEmpty(input.Category))
			query = query.Where(p => p.Category != null && p.Category.Contains(input.Category));

		if (!string.IsNullOrEmpty(input.Category))
		{
			bool inStock = bool.Parse(input.Category);
			query = query.Where(p => p.CurrentStock > 0 == inStock);
		}


		var products = query
			.Select(p => new ProductsDTO
			{
				Id = p.Id,
				Category = p.Category,
				Name = p.Name,
				InStock = p.CurrentStock != 0
			}).Take(input.Results);

		return new RestDTO<ProductsDTO[]>
		{
			Data = await products.ToArrayAsync(),
			Results = input.Results,
			Category = input.Category,
			Available = input.Available,
			Links = new List<LinkDTO>
			{
				new LinkDTO
				(
					Url.Action(null, "products", null, Request.Scheme)!,
					"self",
					"GET"
				)
			}
		};
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