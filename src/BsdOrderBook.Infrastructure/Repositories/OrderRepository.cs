
using System.Collections.Concurrent;
using System.Text.Json;
using BsdOrderBook.Domain.Entities;
using BsdOrderBook.Domain.Enums;
using BsdOrderBook.Domain.Repositories;

namespace BsdOrderBook.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    // Use descending order for Bids (highest price first)
    private readonly SortedList<double, List<Order>> _bids;

    // Use default ascending order for Asks (lowest price first)
    private readonly SortedList<double, List<Order>> _asks = [];

    public OrderRepository(string filePath)
    {
        _bids = new(Comparer<double>.Create((a, b) => b.CompareTo(a)));
        _asks = [];
        LoadOrderBooks(filePath);
    }

    public IReadOnlyDictionary<double, List<Order>> GetOrderedAsks()
    {
        return _asks;
    }

    public IReadOnlyDictionary<double, List<Order>> GetOrderedBids()
    {
        return _bids;
    }

    private void LoadOrderBooks(string filePath)
    {
        var orderBooks = new ConcurrentBag<OrderBook>();

        Parallel.ForEach(File.ReadLines(filePath), line =>
        {
            int jsonIndex = line.IndexOf("{");
            if (jsonIndex == -1) return;

            // Using Span<char> to avoid unnecessary string allocations
            var lineSpan = line.AsSpan();
            var orderBookId = lineSpan[..(jsonIndex - 1)].TrimEnd();
            var jsonPart = lineSpan[jsonIndex..];
            var orderBook = JsonSerializer.Deserialize<OrderBook>(jsonPart);
            if (orderBook == null) return;
            orderBook.OrderBookId = orderBookId.ToString();
            orderBooks.Add(orderBook);
        });

        PopulateCollection(OrderType.Buy, orderBooks.SelectMany(x => x.Bids));
        PopulateCollection(OrderType.Sell, orderBooks.SelectMany(x => x.Asks));
    }

    private void PopulateCollection(string type, IEnumerable<OrderEntry> orderEntries)
    {
        var targetList = type == OrderType.Buy ? _bids : _asks;
        foreach (var orderEntry in orderEntries)
        {
            var order = orderEntry.Order;
            if (!targetList.TryGetValue(order.Price, out List<Order>? value))
            {
                value = ([]);
                targetList[order.Price] = value;
            }

            value.Add(order);
        }
    }
}