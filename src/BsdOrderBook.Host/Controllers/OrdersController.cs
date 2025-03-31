using BsdOrderBook.Application.Services;
using BsdOrderBook.Host.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BsdOrderBook.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IValidator<OrderInput> _validator;
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderBookService _orderBookService;

    public OrdersController(ILogger<OrdersController> logger, IValidator<OrderInput> validator, IOrderBookService orderBookService)
    {
        _logger = logger;
        _validator = validator;
        _orderBookService = orderBookService;
    }

    /// <summary>
    /// Creates a new order and executes it based on available liquidity.
    /// </summary>
    /// <param name="orderInput">The order details.</param>
    /// <returns>Returns the execution plan if successful.</returns>
    /// <response code="200">Order successfully executed.</response>
    /// <response code="400">Validation failed (e.g., missing or incorrect input).</response>
    /// <response code="422">Not enough liquidity to fulfill the order.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ExecutionPlan), 200)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
    [ProducesResponseType(typeof(string), 422)]
    public async Task<IActionResult> CreateOrder(OrderInput orderInput)
    {
        ValidationResult result = await _validator.ValidateAsync(orderInput);
        if (!result.IsValid) 
        {
            return BadRequest(result.ToDictionary());
        }

        var executionOutput = _orderBookService.GetBestExecution(orderInput.OrderType, orderInput.BtcAmount);
        if (executionOutput.HasError)
        {
            return UnprocessableEntity(string.Join(',', executionOutput.ErrorMessages));
        }

        return Ok(new ExecutionPlan(executionOutput.Output, orderInput.BtcAmount, orderInput.OrderType));
    } 

}