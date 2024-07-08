using System.Net;
using FluentAssertions;
using GroceryStoreApi.Controllers;
using GroceryStoreApi.DTO.Cart;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using GroceryStoreTests.Fakes;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStoreTests.Controllers;

public class CartsControllerTests : IDisposable
{
	private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();

	public void Dispose()
	{
		_ctxBuilder.Dispose();
	}

	[Fact]
	public async Task Post_CreatesNewCart_ReturnsCreatedAtAction()
	{
		// Arrange
		var context = _ctxBuilder.WithOneCart().Build();
		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		var result = await _sut.Post();

		// Assert
		var postResult = result as CreatedAtActionResult;
		postResult.Should().NotBeNull();
		postResult?.Value.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCart_ExistingCartId_ReturnsOk()
	{
		// Arrange
		var context = _ctxBuilder.WithOneCart().Build();
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		var result = await _sut.GetCart(cartId);

		// Assert
		var getResult = result as OkResult;
		getResult.Should().NotBeNull();
	}

	[Fact]
	public async Task PostItemToCart_ValidInput_AddsTransaction()
	{
		// Arrange
		var context = _ctxBuilder.WithCarts().WithProducts().Build();
		var cartId = "2E892988-18F1-4DA7-2252-1FB697891A58";
		var cartDTO = new CartDTO
		{
			productId = 4646,
			quantity = 3
		};
		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		var result = await _sut.PostItemToCart(cartId, cartDTO);

		// Assert
		var postResult = result as CreatedResult;
		postResult.Should().BeNull();
		postResult?.StatusCode.Should().Be((int)HttpStatusCode.Created);

		var transaction = await context.Transactions.FirstOrDefaultAsync();
		transaction.Should().NotBeNull();
		transaction?.CartId.Should().Be(cartId);
		transaction?.ProductId.Should().Be(cartDTO.productId);
		transaction?.Quantity.Should().Be(cartDTO.quantity);
	}

	[Fact]
	public async Task GetItemsInCart_ValidCartId_ReturnsAllItems()
	{
		// Arrange
		var context = _ctxBuilder.WithCarts().WithProducts().WithTransactions().Build();
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		var result = await _sut.GetItemsInCart(cartId);

		// Assert
		var getResult = result as OkObjectResult;
		getResult.Should().NotBeNull();
		var items = getResult.Value as IEnumerable<dynamic>;
		items.Should().NotBeNull();
		items.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task UpdateItemInCart_ExistingTransaction_UpdatesTransactionQuantity()
	{
		// Arrange
		var context = _ctxBuilder.WithCarts().WithProducts().WithTransactions().Build();
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
		var itemId = 4646;
		var input = new CartQuantityDTO { Quantity = 3 };

		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		await _sut.UpdateItemInCart(cartId, itemId, input);

		// Assert
		var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.ProductId == itemId);
		transaction.Should().NotBeNull();
		transaction.Quantity.Should().Be(3);
	}

	[Fact]
	public async Task DeleteItemInCart_ExistingTransaction_RemovesTransaction()
	{
		// Arrange
		var context = _ctxBuilder.WithCarts().WithProducts().WithTransactions().Build();
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
		var itemId = 4646;
		var cartDTO = new CartDTO
		{
			productId = itemId,
			quantity = 1
		};

		var cartService = new CartService(context);
		var transactionService = new TransactionService(context);
		var _sut = new CartsController(cartService, transactionService);

		// Act
		await _sut.DeleteItemInCart(cartId, cartDTO);

		// Assert
		var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.ProductId == itemId);
		transaction.Should().BeNull();
	}

}