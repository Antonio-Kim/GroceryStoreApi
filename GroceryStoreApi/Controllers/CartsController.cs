using System.Net;
using GroceryStoreApi.DTO.Cart;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(NoStore = true)]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ITransactionService _transactionService;

    public CartsController(ICartService cartService, ITransactionService transactionService)
    {
        _cartService = cartService;
        _transactionService = transactionService;
    }

    [HttpPost(Name = "Create Cart")]
    public async Task<IActionResult> Post()
    {
        var cart = await _cartService.NewCartAsync();
        return CreatedAtAction(nameof(Post), new { created = true, cartId = cart });
    }

    [HttpGet("{cartId}", Name = "Get Cart")]
    public async Task<IActionResult> GetCart(string cartId)
    {
        var cart = await _cartService.GetCartAsync(cartId);

        if (cart == null)
        {
            return NotFound($"No cart with {cartId} exists.");
        }

        var cartItems = await _transactionService.GetCart(cartId);

        var cartInfo = new
        {
            items = cartItems ?? [],
            created = cart.Created,
        };
        return Ok(cartInfo);
    }

    [HttpPost("{cartId}/items", Name = "Add Item to cart")]
    public async Task<IActionResult> PostItemToCart(string cartId, [FromBody] CartDTO input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var isCreated = await _transactionService.AddItem(cartId, input.productId, input.quantity);

        var newCart = new
        {
            Created = isCreated,
            ItemId = input.productId
        };

        return CreatedAtRoute("Add Item to cart", new { cartId = cartId }, newCart);
    }

    [HttpGet("{cartId}/items", Name = "GetItemsInCart")]
    public async Task<IActionResult> GetItemsInCart(string cartId)
    {
        var cart = await _transactionService.GetCart(cartId);
        if (cart == null)
        {
            return NotFound($"Cart ID {cartId} has done no actions.");
        }

        return Ok(cart);
    }

    [HttpPatch("{cartId}/items/{itemId}")]
    public async Task<IActionResult> UpdateItemInCart(string cartId, int itemId, [FromBody] CartQuantityDTO input)
    {
        var isUpdated = await _transactionService.UpdateCart(cartId, itemId, itemId, input.Quantity);

        if (!isUpdated)
        {
            return NotFound("Item was not found");
        }

        return NoContent();
    }

    [HttpPut("{cartId}/items/{itemId}")]
    public async Task<IActionResult> ReplaceItemInCart(string cartId, int itemId, [FromBody] CartDTO input)
    {
        var isUpdated = await _transactionService.UpdateCart(cartId, itemId, input.productId, input.quantity);

        if (!isUpdated)
        {
            return NotFound("Item was not found");
        }

        return NoContent();
    }

    [HttpDelete("{cartId}/items/{itemId}")]
    public async Task<IActionResult> DeleteItemInCart(string cartId, int itemId, [FromBody] int quantity = 1)
    {
        var isDeleted = await _transactionService.RemoveItem(cartId, itemId, quantity);
        if (!isDeleted)
        {
            return NotFound("Item was not found");
        }

        return NoContent();
    }
}