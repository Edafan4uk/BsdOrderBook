using System.ComponentModel.DataAnnotations;
using BsdOrderBook.Domain.Enums;
using FluentValidation;

namespace BsdOrderBook.Host.Models;

public class OrderInput
{
    /// <summary>
    /// Type of order. Must be either "Buy" or "Sell".
    /// </summary>
    /// <example>Buy</example>
    [Required]
    public string OrderType { get; set; }
    
    /// <summary>
    /// Amount of BTC to trade. Must be greater than zero.
    /// </summary>
    /// <example>1.5</example>
    [Required]
    public double BtcAmount { get; set; }
}

public class OrderInputValidator : AbstractValidator<OrderInput>
{
    public OrderInputValidator()
    {
        RuleFor(o => o.OrderType)
            .NotEmpty().WithMessage("OrderType is required.")
            .Must(orderType => orderType == OrderType.Buy || orderType == OrderType.Sell)
            .WithMessage($"OrderType must be either '{OrderType.Buy}' or '{OrderType.Sell}'.");

        RuleFor(o => o.BtcAmount)
            .GreaterThan(0).WithMessage("BtcAmount must be greater than zero.");
    }
}