using BsdOrderBook.Application.Dto;
using BsdOrderBook.Domain.Enums;
using BsdOrderBook.Domain.Repositories;

namespace BsdOrderBook.Application.Services;

public interface IOrderBookService
{
    ServiceOutput<List<ExecutionOrder>> GetBestExecution(string orderType, double btcAmount);
}

public class OrderBookService : IOrderBookService
{
    private readonly IOrderRepository _orderRepository;

    public OrderBookService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public ServiceOutput<List<ExecutionOrder>> GetBestExecution(string orderType, double btcAmount)
    {
        var executionPlan = new List<ExecutionOrder>();

        // Buy → Get Asks, Sell → Get Bids
        var targetList = orderType == OrderType.Buy
            ? _orderRepository.GetOrderedAsks() : _orderRepository.GetOrderedBids();
        var remainingAmount = btcAmount;
        foreach (var priceLevel in targetList)
        {
            foreach (var order in priceLevel.Value)
            {
                double tradeAmount = Math.Min(order.Amount, remainingAmount);
                executionPlan.Add(new ExecutionOrder(order.Price, tradeAmount));

                remainingAmount -= tradeAmount;
                if (remainingAmount <= 0)
                {
                    return ServiceOutput<List<ExecutionOrder>>.Success(executionPlan);
                }
            }
        }

        // If not enough liquidity, return null even if some orders were matched
        return ServiceOutput<List<ExecutionOrder>>.Failure("Not enough liquidity to fulfill the order.");
    }
}