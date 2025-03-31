using System.Collections.Concurrent;
using System.Text.Json;
using BsdOrderBook.Console;

class Program
{

    private static SortedList<double, List<Order>> Bids = new(Comparer<double>.Create((a, b) => b.CompareTo(a)));

    // Use default ascending order for Asks (lowest price first)
    private static SortedList<double, List<Order>> Asks = [];

    static void Main(string[] args)
    {
        // var line = File.ReadLines("../../order_books_data").First();
        // int jsonIndex = line.IndexOf("{");
        // if (jsonIndex == -1) return;
        // var lineSpan = line.AsSpan();
        // var orderBookId = lineSpan.Slice(0, jsonIndex - 1).TrimEnd();
        // var jsonPart = lineSpan.Slice(jsonIndex);
        // System.Console.WriteLine($"\"{orderBookId}\"");
        // System.Console.WriteLine(jsonPart.ToString());
        // string filePath = "order_books.json";
        LoadOrderBooks("../../order_books_data");
        Console.WriteLine($"Loaded {Bids.Count} order books.");
        Console.WriteLine($"Loaded {Asks.Count} order books.");
        var buyAmount = 9;
        var buyOrders = GetBestExecution("Buy", buyAmount);
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

    public static ServiceOutput<List<ExecutionOrder>> GetBestExecution(string orderType, double btcAmount)
    {
        var executionPlan = new List<ExecutionOrder>();
        var targetList = orderType == "Buy" ? Asks : Bids; // Buy → Get Asks, Sell → Get Bids

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
