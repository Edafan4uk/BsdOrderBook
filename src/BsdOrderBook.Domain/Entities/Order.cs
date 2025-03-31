namespace BsdOrderBook.Domain.Entities;

public class Order
{
    public string? OrderBookId { get; set; }
    public required string Type { get; set; }
    public required string Kind { get; set; }
    public double Amount { get; set; }
    public double Price { get; set; }
}