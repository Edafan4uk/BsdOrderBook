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
            var totalPrice = buyOrders.Output.Sum(x => x.Amount * x.Price);
            System.Console.WriteLine($"Btc amount: {buyAmount}; Total price: {totalPrice}");
        }
        else
        {
            System.Console.WriteLine(buyOrders);
        }
    }
}
