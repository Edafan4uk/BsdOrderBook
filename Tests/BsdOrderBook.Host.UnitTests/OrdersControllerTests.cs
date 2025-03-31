using BsdOrderBook.Application.Dto;
using BsdOrderBook.Application.Services;
using BsdOrderBook.Host.Controllers;
using BsdOrderBook.Host.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BsdOrderBook.Host.UnitTests;

public class OrdersControllerTests
{
    private readonly IValidator<OrderInput> _validator;
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderBookService _orderBookService;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _validator = Substitute.For<IValidator<OrderInput>>();
        _logger = Substitute.For<ILogger<OrdersController>>();
        _orderBookService = Substitute.For<IOrderBookService>();
        _controller = new OrdersController(_logger, _validator, _orderBookService);
    }

    [Fact]
    public async Task CreateOrder_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var orderInput = new OrderInput { OrderType = "Invalid", BtcAmount = -1 };
        var validationFailures = new List<ValidationFailure>
        {
            new("OrderType", "Invalid order type."),
            new("BtcAmount", "Amount must be greater than zero.")
        };
        _validator.ValidateAsync(orderInput).Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _controller.CreateOrder(orderInput);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var errors = badRequestResult.Value as Dictionary<string, string[]>;
        errors.Should().ContainKey("OrderType");
        errors.Should().ContainKey("BtcAmount");
    }

    [Fact]
    public async Task CreateOrder_WhenNotEnoughLiquidity_ReturnsUnprocessableEntity()
    {
        // Arrange
        var orderInput = new OrderInput { OrderType = "Buy", BtcAmount = 1.0 };
        _validator.ValidateAsync(orderInput).Returns(new ValidationResult());
        _orderBookService.GetBestExecution(orderInput.OrderType, orderInput.BtcAmount)
            .Returns(ServiceOutput<List<ExecutionOrder>>.Failure("Not enough liquidity to fulfill the order."));

        // Act
        var result = await _controller.CreateOrder(orderInput);

        // Assert
        var unprocessableResult = result.Should().BeOfType<UnprocessableEntityObjectResult>().Subject;
        unprocessableResult.Value.Should().Be("Not enough liquidity to fulfill the order.");
    }

    [Fact]
    public async Task CreateOrder_WhenValidOrder_ReturnsOkWithExecutionPlan()
    {
        // Arrange
        var orderInput = new OrderInput { OrderType = "Sell", BtcAmount = 0.5 };
        var executionOrders = new List<ExecutionOrder>
        {
            new(2960.64, 0.3),
            new(2964.29, 0.2)
        };
        _validator.ValidateAsync(orderInput).Returns(new ValidationResult());
        _orderBookService.GetBestExecution(orderInput.OrderType, orderInput.BtcAmount)
            .Returns(ServiceOutput<List<ExecutionOrder>>.Success(executionOrders));

        // Act
        var result = await _controller.CreateOrder(orderInput);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var executionPlan = okResult.Value as ExecutionPlan;
        executionPlan.Should().NotBeNull();
        executionPlan.ExecutionOrders.Should().HaveCount(2);
        executionPlan.BtcAmount.Should().Be(0.5);
        executionPlan.TotalPrice.Should().BeApproximately(2960.64 * 0.3 + 2964.29 * 0.2, 0.01);
    }
}
