using FluentAssertions;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using GroceryStoreTests.Fakes;

namespace GroceryStoreTests.Services;

public class OrderServiceTests : IDisposable
{
	private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
	private OrderService? _sut;

	public void Dispose()
	{
		_ctxBuilder.Dispose();
	}

	[Fact]
	public async Task GetAllOrders_OrdersInTable_ReturnsAllOrders()
	{
		// Arrange
		var context = _ctxBuilder
			.WithCarts()
			.WithProducts()
			.WithTransactions()
			.WithOrders()
			.Build();
		var cartService = new CartService(context);
		var _sut = new OrderService(context, cartService);
		var orderOne = new Order
		{
			OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
			CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
			CustomerName = "John Doe",
			Comment = ""
		};
		var orderTwo = new Order
		{
			OrderId = Guid.Parse("5A683325-73DF-882A-351E-2E924AE8EC3F"),
			CartId = Guid.Parse("2E892988-18F1-4DA7-2252-1FB697891A58"),
			CustomerName = "Jane Doe",
			Comment = "Express Delivery"
		};

		// Act
		var result = await _sut.GetAllOrders();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result[0].Should().BeEquivalentTo(orderOne);
		result[1].Should().BeEquivalentTo(orderTwo);
	}

	[Fact]
	public async Task GetOrder_ValidOrderId_ReturnOrder()
	{
		var context = _ctxBuilder
			.WithCarts()
			.WithProducts()
			.WithTransactions()
			.WithOrders()
			.Build();
		var cartService = new CartService(context);
		var _sut = new OrderService(context, cartService);
		var orderId = "2F683325-73DF-882A-351E-2E924AE8EC3C";
		var order = new Order
		{
			OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
			CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
			CustomerName = "John Doe",
			Comment = ""
		};

		// Act
		var result = await _sut.GetOrder(orderId);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(order);
	}

	[Fact]
	public async Task GetOrder_InvalidOrderId_ReturnsNull()
	{
		// Arrange
		var context = _ctxBuilder
			.WithCarts()
			.WithProducts()
			.WithTransactions()
			.WithOrders()
			.Build();
		var cartService = new CartService(context);
		var _sut = new OrderService(context, cartService);

		// Act
		var result = await _sut.GetOrder("12524124");

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task AddOrder_ValidInput_ReturnsTrue()
	{
		// Arrange
		var context = _ctxBuilder
			.WithCarts()
			.WithProducts()
			.WithTransactions()
			.WithOrders()
			.Build();
		var cartService = new CartService(context);
		var _sut = new OrderService(context, cartService);
		var cartId = "1C892986-18F1-82AD-2252-1FB6978922FA";
		var customerName = "Jane Smith";
		var comment = "";

		// Act
		var result = await _sut.CreateOrder(cartId, customerName, comment);

		// Assert
		result.Should().BeTrue();
		var cart = await cartService.GetCartAsync(cartId);
		cart.Should().BeNull();
	}

	[Fact]
	public async Task AddOrder_InvalidCartId_ReturnsTrue()
	{
		// Arrange
		var context = _ctxBuilder
			.WithCarts()
			.WithProducts()
			.WithTransactions()
			.WithOrders()
			.Build();
		var cartService = new CartService(context);
		var _sut = new OrderService(context, cartService);
		var cartId = "1C892986-18F1-82AD-2252-1FB6978922FB";
		var customerName = "Jane Smith";
		var comment = "";

		// Act
		var result = await _sut.CreateOrder(cartId, customerName, comment);

		// Assert
		result.Should().BeFalse();
	}
}