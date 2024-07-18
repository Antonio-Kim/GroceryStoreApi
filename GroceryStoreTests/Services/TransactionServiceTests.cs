using FluentAssertions;
using GroceryStoreApi.Models;
using GroceryStoreApi.Services;
using GroceryStoreTests.Fakes;

namespace GroceryStoreTests.Services;

public class TransactionServiceTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    private TransactionService? _sut;

    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetCart_CorrectCartId_ReturnsList()
    {
        var ctx = _ctxBuilder.WithTransactions().Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        _sut = new TransactionService(ctx);

        var actual = await _sut.GetCart(cartId);

        actual.Should().NotBeNull();
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetCart_IncorrectId_ReturnsNull()
    {
        var ctx = _ctxBuilder.WithTransactions().Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A50";
        _sut = new TransactionService(ctx);

        var actual = await _sut.GetCart(cartId);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetTransactions_CorrectCartIdItemId_ReturnsTransaction()
    {
        var context = _ctxBuilder.WithTransactions().Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 2585;
        var expected = new Transactions
        {
            CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
            ProductId = 2585,
            Quantity = 4
        };
        var _sut = new TransactionService(context);

        var actual = await _sut.GetTransaction(cartId, itemId);

        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetTransactions_IncorrectCartId_ReturnsNull()
    {
        var context = _ctxBuilder.WithTransactions().Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A59";
        var itemId = 2585;

        var _sut = new TransactionService(context);

        var actual = await _sut.GetTransaction(cartId, itemId);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetTransactions_IncorrectItemId_ReturnsNull()
    {
        var context = _ctxBuilder.WithTransactions().Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 1111;

        var _sut = new TransactionService(context);

        var actual = await _sut.GetTransaction(cartId, itemId);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task AddItem_CorrectCartIdItemId_ReturnsTrue()
    {
        var context = _ctxBuilder.WithCarts().WithProducts().Build();
        var _sut = new TransactionService(context);
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 2585;
        var newTransaction = new Transactions
        {
            CartId = Guid.Parse(cartId),
            ProductId = itemId,
            Quantity = 1
        };

        var flag = await _sut.AddItem(cartId, itemId);

        flag.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newTransaction, options => options
            .ExcludingNestedObjects()
            .Excluding(x => x.Cart)
            .Excluding(x => x.Product));
    }

    [Fact]
    public async Task AddItem_IncorrectCartId_ReturnsFalse()
    {
        var context = _ctxBuilder.WithCarts().WithProducts().Build();
        var _sut = new TransactionService(context);
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A59";
        var itemId = 2585;


        var actual = await _sut.AddItem(cartId, itemId);

        actual.Should().BeFalse();
    }

    [Fact]
    public async Task AddItem_IncorrectItemId_ReturnsFalse()
    {
        var context = _ctxBuilder.WithCarts().WithProducts().Build();
        var _sut = new TransactionService(context);
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 2586;


        var actual = await _sut.AddItem(cartId, itemId);

        actual.Should().BeFalse();
    }

    [Fact]
    public async void RemoveItem_WithOneItemQuantity_RemovesTransaction()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 4646;
        var _sut = new TransactionService(context);

        var actual = await _sut.RemoveItem(cartId, itemId);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result.Should().BeNull();
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(2);
    }

    [Fact]
    public async void RemoveItem_WithMultipleItems_RemovesOneQuantity()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 2585;
        var _sut = new TransactionService(context);
        var expectedResult = new Transactions
        {
            CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
            ProductId = 2585,
            Quantity = 3
        };

        var actual = await _sut.RemoveItem(cartId, itemId);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result.Should().NotBeNull();
        result?.Quantity.Should().Be(expectedResult.Quantity);
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void RemoveItem_WithMultipleItems_RemovesMultipleQuantity()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 2585;
        var _sut = new TransactionService(context);
        var expectedResult = new Transactions
        {
            CartId = Guid.Parse("1C892986-18F1-4DA7-2252-1FB697891A58"),
            ProductId = 2585,
            Quantity = 2
        };

        var actual = await _sut.RemoveItem(cartId, itemId, 2);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result?.Quantity.Should().Be(expectedResult.Quantity);
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void RemoveItem_RemovingMoreItems_ReturnsNull()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 4646;
        var _sut = new TransactionService(context);

        var actual = await _sut.RemoveItem(cartId, itemId, 2);

        actual.Should().BeFalse();
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void RemoveItem_RemovingExactQuantity_RemovesTransaction()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 3674;
        var _sut = new TransactionService(context);

        var actual = await _sut.RemoveItem(cartId, itemId, 8);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result.Should().BeNull();
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(2);
    }

    [Fact]
    public async void RemoveItem_IncorrectCartId_ReturnsFalse()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A59";
        var itemId = 4646;
        var _sut = new TransactionService(context);

        var actual = await _sut.RemoveItem(cartId, itemId);

        actual.Should().BeFalse();
        var cart = await _sut.GetCart(cartId);
    }

    [Fact]
    public async void RemoveItem_IncorrectItemId_ReturnsFalse()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 4645;
        var _sut = new TransactionService(context);

        var actual = await _sut.RemoveItem(cartId, itemId);

        actual.Should().BeFalse();
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void UpdateCart_CorrectItemIds_ReturnTrue()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 3674;
        var newItemId = 9482;
        var _sut = new TransactionService(context);
        var expectedResult = new Transactions
        {
            CartId = Guid.ParseExact(cartId, "D"),
            ProductId = newItemId,
            Quantity = 1
        };

        var actual = await _sut.UpdateCart(cartId, itemId, newItemId);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result?.CartId.Should().Be(expectedResult.CartId);
        result?.ProductId.Should().Be(expectedResult.ProductId);
        result?.Quantity.Should().Be(expectedResult.Quantity);
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void UpdateCart_MultipleQuantity_ReturnTrue()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 3674;
        var newItemId = 9482;
        var _sut = new TransactionService(context);
        var expectedResult = new Transactions
        {
            CartId = Guid.ParseExact(cartId, "D"),
            ProductId = newItemId,
            Quantity = 4
        };

        var actual = await _sut.UpdateCart(cartId, itemId, newItemId, 4);

        actual.Should().BeTrue();
        var result = await _sut.GetTransaction(cartId, itemId);
        result?.CartId.Should().Be(expectedResult.CartId);
        result?.ProductId.Should().Be(expectedResult.ProductId);
        result?.Quantity.Should().Be(expectedResult.Quantity);
        var cart = await _sut.GetCart(cartId);
        cart.Should().HaveCount(3);
    }

    [Fact]
    public async void UpdateCart_IncorrectItemId_ReturnFalse()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A58";
        var itemId = 3674;
        var newItemId = 4645;
        var _sut = new TransactionService(context);

        var actual = await _sut.UpdateCart(cartId, itemId, newItemId);

        actual.Should().BeFalse();
    }

    [Fact]
    public async void UpdateCart_IncorrectCartId_ReturnFalse()
    {
        var context = _ctxBuilder.
                WithTransactions().
                WithCarts().
                WithProducts().
            Build();
        var cartId = "1C892986-18F1-4DA7-2252-1FB697891A50";
        var itemId = 3674;
        var newItemId = 4645;
        var _sut = new TransactionService(context);

        var actual = await _sut.UpdateCart(cartId, itemId, newItemId);

        actual.Should().BeFalse();
    }
}