using GroceryStoreApi.DTO.Order;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreApi.Controllers;

[ApiController]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public class OrdersController : ControllerBase
{
	private readonly ICartService _cartService;
	private readonly IOrderService _orderService;
	public OrdersController(CartService cartService, OrderService orderService)
	{
		_cartService = cartService;
		_orderService = orderService;
	}

	[HttpGet(Name = "Get all orders")]
	public async Task<IActionResult> GetOrders()
	{
		try
		{
			var orders = await _orderService.GetAllOrders();
			if (orders == null || !orders.Any()) return NotFound();
			return Ok(orders);
		}
		catch (Exception ex)
		{
			throw new Exception($"Exception occurred when fetching orders: ${ex.Message}");
		}
	}

	[HttpGet("{orderId}", Name = "Get a single order")]
	public async Task<IActionResult> GetOrder(string orderId)
	{
		try
		{
			var order = await _orderService.GetOrder(orderId);
			if (order == null) return NotFound();

			return Ok(order);
		}
		catch (Exception ex)
		{
			throw new Exception($"Exception occurred when fetching orders: ${ex.Message}");
		}
	}

	[HttpPost(Name = "Create a new order")]
	public async Task<IActionResult> PostOrder(string cartId, [FromBody] OrderDTO input)
	{
		try
		{
			var order = await _orderService.CreateOrder(cartId, input.CustomerName, input.Comment);
			if (!order) return BadRequest("Not valid ");

			return StatusCode(201);
		}
		catch (Exception ex)
		{
			throw new Exception($"Exception occurred when creating order: ${ex.Message}");
		}
	}
}