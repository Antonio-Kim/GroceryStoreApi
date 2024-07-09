using FluentAssertions;
using GroceryStoreApi.Controllers;
using GroceryStoreApi.DTO.Order;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using GroceryStoreTests.Fakes;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreTests.Controllers;

public class OrdersControllerTests : IDisposable
{
	private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();

	public void Dispose()
	{
		_ctxBuilder.Dispose();
	}

	[Fact]
	public async Task Get_AllOrders_ReturnsInList()
	{
		// Arrange
		var context = _ctxBuilder.WithOrders().Build();
		var cartService = new CartService(context);
		var orderService = new OrderService(context, cartService);
		var _sut = new OrdersController(cartService, orderService);
		var orders = new List<Order>
		{
			new Order
			{
				OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
				CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
				CustomerName = "John Doe",
				Comment = ""
			},
			new Order
			{
				OrderId = Guid.Parse("5A683325-73DF-882A-351E-2E924AE8EC3F"),
				CartId = Guid.Parse("2E892988-18F1-4DA7-2252-1FB697891A58"),
				CustomerName = "Jane Doe",
				Comment = "Express Delivery"
			}
		};

		// Act
		var result = await _sut.GetOrders();

		// Assert
		var getResult = result as OkObjectResult;
		getResult.Should().NotBeNull();
		getResult?.StatusCode.Should().Be(200);
		var returnedOrders = getResult?.Value as List<Order>;
		returnedOrders.Should().NotBeNull();
		returnedOrders.Should().BeEquivalentTo(orders);
	}

	[Fact]
	public async Task GetOrder_ValidOrderId_ReturnsOrder()
	{
		// Arrange
		var context = _ctxBuilder.WithOrders().Build();
		var cartService = new CartService(context);
		var orderService = new OrderService(context, cartService);
		var _sut = new OrdersController(cartService, orderService);
		var order = new Order
		{
			OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
			CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
			CustomerName = "John Doe",
			Comment = ""
		};

		// Act
		var result = await _sut.GetOrder(order.OrderId.ToString());

		// Assert
		var getResult = result as OkObjectResult;
		getResult.Should().NotBeNull();
		getResult?.StatusCode.Should().Be(200);
		var returnedOrder = getResult?.Value as Order;
		returnedOrder.Should().NotBeNull();
		returnedOrder.Should().BeEquivalentTo(order);
	}

	[Fact]
	public async Task GetOrder_InvalidOrderId_ReturnsNotFound()
	{
		// Arrange
		var context = _ctxBuilder.WithOrders().Build();
		var cartService = new CartService(context);
		var orderService = new OrderService(context, cartService);
		var _sut = new OrdersController(cartService, orderService);
		var order = new Order
		{
			OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3D"),
			CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
			CustomerName = "John Doe",
			Comment = ""
		};

		// Act
		var result = await _sut.GetOrder(order.OrderId.ToString());

		// Assert
		var getResult = result as NotFoundResult;
		getResult.Should().NotBeNull();
		getResult?.StatusCode.Should().Be(404);
	}

	[Fact]
	public async Task PostOrder_ValidCartId_ReturnsCreated()
	{
		// Arrange
		var context = _ctxBuilder.WithOneCart().WithTransactions().WithProducts().Build();
		var cartService = new CartService(context);
		var orderService = new OrderService(context, cartService);
		var _sut = new OrdersController(cartService, orderService);
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
		var input = new OrderDTO
		{
			CustomerName = "Jack Black"
		};

		// Act
		var result = await _sut.PostOrder(cartId, input);

		// Assert
		var postResult = result as StatusCodeResult;
		postResult.Should().NotBeNull();
		postResult?.StatusCode.Should().Be(201);
		var orders = await orderService.GetAllOrders();
		orders.Should().NotBeNull();
		orders.Should().HaveCount(1);
		orders[0].CartId.Should().Be(cartId);
		orders[0].CustomerName.Should().Be(input.CustomerName);
		orders[0].Comment.Should().NotBeNull();

	}

	[Fact]
	public async Task PostOrder_InvalidCartId_ReturnsBadRequest()
	{
		// Arrange
		var context = _ctxBuilder.WithOneCart().WithTransactions().WithProducts().Build();
		var cartService = new CartService(context);
		var orderService = new OrderService(context, cartService);
		var _sut = new OrdersController(cartService, orderService);
		var cartId = "1C892986-18F1-4DA7-2252-1FB697891A59";
		var input = new OrderDTO
		{
			CustomerName = "Jack Black"
		};

		// Act
		var result = await _sut.PostOrder(cartId, input);

		// Assert
		var postResult = result as BadRequestObjectResult;
		postResult.Should().NotBeNull();
		postResult?.StatusCode.Should().Be(400);
	}
}