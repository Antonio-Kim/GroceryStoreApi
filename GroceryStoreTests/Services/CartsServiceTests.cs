using FluentAssertions;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using GroceryStoreTests.Fakes;

namespace GroceryStoreTests.Services;

public class CartServicesTests : IDisposable
{
	private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
	private CartService? _sut;

	[Fact]
	public async Task GetCartAsync_NoCart_ReturnsNull()
	{
		var ctx = _ctxBuilder.Build();
		_sut = new CartService(ctx);


		var actual = await _sut.GetCartAsync("1C892986-18F1-4DA7-2252-1FB697891A58");

		actual.Should().BeNull();
	}

	[Fact]
	public async Task GetCartAsync_WithCarts_ReturnsCart()
	{
		var ctx = _ctxBuilder.WithCarts().Build();
		_sut = new CartService(ctx);

		var actual = await _sut.GetCartAsync("1C892986-18F1-4DA7-2252-1FB697891A58");

		actual.Should().NotBeNull();
		actual?.CartId.Should().Be(new Guid("1C892986-18F1-4DA7-2252-1FB697891A58"));
	}

	[Fact]
	public async Task GetCartAsync_WithCartsIncorrectCart_ReturnsNull()
	{
		var ctx = _ctxBuilder.WithCarts().Build();
		_sut = new CartService(ctx);

		var actual = await _sut.GetCartAsync("1C892986-18F1-4DA7-2252-1FB697891A59");

		actual.Should().BeNull();
	}

	public void Dispose()
	{
		_ctxBuilder.Dispose();
	}
}