using BsdOrderBook.Domain.Entities;

namespace BsdOrderBook.Domain.Repositories;

public interface IOrderRepository
{
    IReadOnlyDictionary<double, List<Order>> GetOrderedBids();
    IReadOnlyDictionary<double, List<Order>> GetOrderedAsks();
}