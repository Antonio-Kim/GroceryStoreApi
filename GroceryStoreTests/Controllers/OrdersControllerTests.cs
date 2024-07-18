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
        var input = new OrderDTO
        {
            CartId = "1C892986-18F1-4DA7-2252-1FB697891A58",
            CustomerName = "Jack Black"
        };

        // Act
        var result = await _sut.PostOrder(input);

        // Assert
        var postResult = result as StatusCodeResult;
        postResult.Should().NotBeNull();
        postResult?.StatusCode.Should().Be(201);
        var orders = await orderService.GetAllOrders();
        orders.Should().NotBeNull();
        orders.Should().HaveCount(1);
        orders[0].CartId.Should().Be(input.CartId);
        orders[0].CustomerName.Should().Be(input.CustomerName);
        orders[0].Comment.Should().NotBeNull();
        var cart = await cartService.GetCartAsync(input.CartId);
        cart.Should().BeNull();
    }

    [Fact]
    public async Task PostOrder_InvalidCartId_ReturnsBadRequest()
    {
        // Arrange
        var context = _ctxBuilder.WithOneCart().WithTransactions().WithProducts().Build();
        var cartService = new CartService(context);
        var orderService = new OrderService(context, cartService);
        var _sut = new OrdersController(cartService, orderService);
        var input = new OrderDTO
        {
            CartId = "1C892986-18F1-4DA7-2252-1FB697891A59",
            CustomerName = "Jack Black"
        };

        // Act
        var result = await _sut.PostOrder(input);

        // Assert
        var postResult = result as BadRequestObjectResult;
        postResult.Should().NotBeNull();
        postResult?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateOrder_ValidCartId_ReturnsNoContent()
    {
        // Arrange
        var context = _ctxBuilder
            .WithOrders()
            .WithOneCart()
            .WithTransactions()
            .WithProducts()
            .Build();
        var cartService = new CartService(context);
        var orderService = new OrderService(context, cartService);
        var _sut = new OrdersController(cartService, orderService);
        var orderId = "2F683325-73DF-882A-351E-2E924AE8EC3C";
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var input = new OrderDTO
        {
            CustomerName = "Jack Black",
            Comment = "Next-day delivery"
        };

        // Act
        var result = await _sut.UpdateOrder(orderId, input);

        // Assert
        var patchResult = result as NoContentResult;
        patchResult.Should().NotBeNull();
        patchResult?.StatusCode.Should().Be(204);
        var orders = await orderService.GetAllOrders();
        orders.Should().NotBeNull();
        orders.Should().HaveCount(2);
        orders[0].OrderId.Should().Be(orderId);
        orders[0].CartId.Should().Be(cartId);
        orders[0].CustomerName.Should().Be(input.CustomerName);
        orders[0].Comment.Should().Be(input.Comment);
    }

    [Fact]
    public async Task UpdateOrder_InvalidOrderId_ReturnsNotFound()
    {
        // Arrange
        var context = _ctxBuilder
            .WithOrders()
            .WithOneCart()
            .WithTransactions()
            .WithProducts()
            .Build();
        var cartService = new CartService(context);
        var orderService = new OrderService(context, cartService);
        var _sut = new OrdersController(cartService, orderService);
        var orderId = "2F683325-73DF-882A-351E-2E924AE8EC3C";
        var input = new OrderDTO
        {
            CustomerName = "Jack Black",
            Comment = "Next-day delivery"
        };

        // Act
        var result = await _sut.UpdateOrder(orderId, input);

        // Assert
        var patchResult = result as NotFoundObjectResult;
        patchResult.Should().BeNull();
        patchResult?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task DeleteOrder_ValidOrderId_ReturnsNoContent()
    {
        // Arrange
        var context = _ctxBuilder
            .WithOrders()
            .WithOneCart()
            .WithTransactions()
            .WithProducts()
            .Build();
        var cartService = new CartService(context);
        var orderService = new OrderService(context, cartService);
        var _sut = new OrdersController(cartService, orderService);
        var orderId = "5A683325-73DF-882A-351E-2E924AE8EC3F";
        var remainingOrder = new Order
        {
            OrderId = Guid.Parse("2F683325-73DF-882A-351E-2E924AE8EC3C"),
            CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
            CustomerName = "John Doe",
            Comment = ""
        };

        // Act
        var result = await _sut.DeleteOrder(orderId);

        // Assert
        var deleteResult = result as NoContentResult;
        deleteResult.Should().NotBeNull();
        deleteResult?.StatusCode.Should().Be(204);
        var orders = await orderService.GetAllOrders();
        orders.Should().NotBeNull();
        orders.Should().HaveCount(1);
        orders[0].Should().BeEquivalentTo(remainingOrder);
    }

    [Fact]
    public async Task DeleteOrder_InvalidOrderId_ReturnsNotFound()
    {
        // Arrange
        var context = _ctxBuilder
            .WithOrders()
            .WithOneCart()
            .WithTransactions()
            .WithProducts()
            .Build();
        var cartService = new CartService(context);
        var orderService = new OrderService(context, cartService);
        var _sut = new OrdersController(cartService, orderService);
        var orderId = "5A683325-73DF-882A-351E-2E924AE8EC3A";

        // Act
        var result = await _sut.DeleteOrder(orderId);

        // Assert
        var deleteResult = result as NotFoundObjectResult;
        deleteResult.Should().NotBeNull();
        deleteResult?.StatusCode.Should().Be(404);
    }
}