using BsdOrderBook.Application.Dto;

namespace BsdOrderBook.Host.Models;

class ExecutionPlan
{
    public double TotalPrice { get; }
    public double BtcAmount { get; }
    public string Type { get; }
    public List<ExecutionOrder> ExecutionOrders { get; set; }

    public ExecutionPlan(List<ExecutionOrder> executionOrders, double totalPrice, double btcAmount, string type)
    {
        ExecutionOrders = executionOrders;
        TotalPrice = totalPrice;
        BtcAmount = btcAmount;
        Type = type;
    }

    public override string ToString() =>
        $"BtcAmount: {BtcAmount}, Type: {Type}, TotalPrice: {TotalPrice} eur, OrderCount: {ExecutionOrders.Count}";
}