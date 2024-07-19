using GroceryStoreApi.DTO.Order;
using GroceryStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(NoStore = true)]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    public OrdersController(ICartService cartService, IOrderService orderService)
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
    public async Task<IActionResult> PostOrder([FromBody] OrderDTO input)
    {
        try
        {
            var order = await _orderService.CreateOrder(input.CartId, input?.CustomerName, input?.Comment);
            if (order == null) return BadRequest("Not valid");

            var response = new
            {
                created = true,
                orderId = order
            };

            return StatusCode(201, response);
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception occurred when creating order: ${ex.Message}");
        }
    }

    [HttpPatch("{orderId}", Name = "Update an order")]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderUpdateDTO input)
    {
        try
        {
            var order = await _orderService.GetOrder(orderId);
            if (order == null)
                return NotFound("There is no order with specified id associated with the API client");
            var newOrder = await _orderService.UpdateOrder(orderId, input.CustomerName, input.Comment);
            if (!newOrder)
                return BadRequest("Parameters provided are invalid");

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception occurred when updating order: ${ex.Message}");
        }
    }

    [HttpDelete("{orderId}", Name = "Delete an order")]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        try
        {
            var order = await _orderService.DeleteOrder(orderId);
            if (!order) return NotFound("There is no order with the specified id associated with the API client");

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception occurred when deleting order: ${ex.Message}");
        }
    }
}