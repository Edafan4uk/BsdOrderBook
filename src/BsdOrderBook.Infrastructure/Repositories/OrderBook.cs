using BsdOrderBook.Domain.Entities;

namespace BsdOrderBook.Infrastructure.Repositories;

class OrderBook
{
    public string? OrderBookId { get; set; }
    public DateTime AcqTime { get; set; }
    public required List<OrderEntry> Bids { get; set; }
    public required List<OrderEntry> Asks { get; set; }
}

class OrderEntry { public required Order Order { get; set; } }