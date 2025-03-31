
using BsdOrderBook.Application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class IocConfig
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IOrderBookService, OrderBookService>();
    }
}