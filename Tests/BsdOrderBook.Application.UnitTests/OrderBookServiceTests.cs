using BsdOrderBook.Application.Services;
using BsdOrderBook.Domain.Entities;
using BsdOrderBook.Domain.Repositories;
using NSubstitute;

namespace BsdOrderBook.Application.UnitTests;

public class OrderBookServiceTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderBookService _orderBookService;

    public OrderBookServiceTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderBookService = new OrderBookService(_orderRepository);
    }

    [Fact]
    public void GetBestExecution_WhenBuyOrder_ReturnsExecutionPlan()
    {
        // Arrange
        var orderInput = "Buy";
        var btcAmount = 0.4;
        var bids = new SortedList<double, List<Order>>()
            {
                { 2960.64, new List<Order> { new Order { Amount = 0.01, Price = 2960.64 } } },
                { 2964.29, new List<Order> { new Order { Amount = 0.4, Price = 2964.29 } } },
                { 3000.29, new List<Order> { new Order { Amount = 1, Price = 3000.29 } } }
            };
        _orderRepository.GetOrderedAsks().Returns(bids);

        // Act
        var result = _orderBookService.GetBestExecution(orderInput, btcAmount);

        // Assert
        Assert.False(result.HasError);
        Assert.NotNull(result.Output);
        Assert.Equal(2, result.Output.Count);
        Assert.Equal(2960.64, result.Output[0].Price);
        Assert.Equal(0.01, result.Output[0].Amount);
        Assert.Equal(2964.29, result.Output[1].Price);
        Assert.Equal(0.39, result.Output[1].Amount);
    }

    [Fact]
    public void GetBestExecution_WhenSellOrder_ReturnsExecutionPlan()
    {
        // Arrange
        var orderInput = "Sell";
        var btcAmount = 0.4;
        var asks = new SortedList<double, List<Order>>()
        {
            { 2964.29, new List<Order> { new Order { Amount = 0.405, Price = 2964.29 } } }
        };
        _orderRepository.GetOrderedBids().Returns(asks);

        // Act
        var result = _orderBookService.GetBestExecution(orderInput, btcAmount);

        // Assert
        Assert.False(result.HasError);
        Assert.NotNull(result.Output);
        Assert.Single(result.Output);
        Assert.Equal(2964.29, result.Output[0].Price);
        Assert.Equal(0.4, result.Output[0].Amount);
    }

    [Fact]
    public void GetBestExecution_WhenNotEnoughLiquidity_ReturnsFailure()
    {
        // Arrange
        var orderInput = "Buy";
        var btcAmount = 1.0;
        var bids = new SortedList<double, List<Order>>()
            {
                { 2960.64, new List<Order> { new Order { Amount = 0.01, Price = 2960.64 } } },
                { 2964.29, new List<Order> { new Order { Amount = 0.2, Price = 2964.29 } } }
            };

        _orderRepository.GetOrderedBids().Returns(bids);

        // Act
        var result = _orderBookService.GetBestExecution(orderInput, btcAmount);

        // Assert
        Assert.True(result.HasError);
        Assert.Null(result.Output);
        Assert.Single(result.ErrorMessages);
        Assert.Equal("Not enough liquidity to fulfill the order.", result.ErrorMessages[0]);
    }
}