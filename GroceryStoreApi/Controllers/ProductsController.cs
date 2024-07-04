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
	public async Task<RestDTO<Product[]>> Get()
	{
		var query = _context.Products;

		return new RestDTO<Product[]>
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
}