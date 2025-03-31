class ExecutionOrder
{
    public double Price { get; }
    public double Amount { get; }

    public ExecutionOrder(double price, double amount)
    {
        Price = price;
        Amount = amount;
    }

    public override string ToString() => $"Price: {Price}, Amount: {Amount}";
}
