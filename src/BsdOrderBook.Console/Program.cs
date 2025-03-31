using BsdOrderBook.Application.Services;
using BsdOrderBook.Domain.Enums;
using BsdOrderBook.Infrastructure.Repositories;

class Program
{

    static void Main(string[] args)
    {
        var repository = new OrderRepository("../../order_books_data");
        var service = new OrderBookService(repository);
        var buyAmount = 9;
        var buyOrders = service.GetBestExecution(OrderType.Buy, buyAmount);
        if (buyOrders.Output != null)
        {
            var executionPlan = new ExecutionPlan(buyOrders.Output, buyAmount, "Buy");
            System.Console.WriteLine(executionPlan);
        }
        else
        {
            System.Console.WriteLine(buyOrders);
        }
    }
}
