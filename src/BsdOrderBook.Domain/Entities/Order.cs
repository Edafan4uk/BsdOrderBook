namespace BsdOrderBook.Domain.Entities;

public class Order
{
    public string? OrderBookId { get; set; }
    public string? Type { get; set; }
    public string? Kind { get; set; }
    public double Amount { get; set; }
    public double Price { get; set; }
}