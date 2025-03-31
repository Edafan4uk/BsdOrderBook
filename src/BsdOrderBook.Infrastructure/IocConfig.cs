using BsdOrderBook.Domain.Repositories;
using BsdOrderBook.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static class IocConfig
{
    public static void AddPersistence(this IServiceCollection services, string orderBookFilePath)
    {
        var repo = new OrderRepository(orderBookFilePath);
        services.AddSingleton<IOrderRepository>(repo);
    }
}