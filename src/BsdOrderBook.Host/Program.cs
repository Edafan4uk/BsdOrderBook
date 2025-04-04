using System.Reflection;
using BsdOrderBook.Host.Models;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddApplication();
var orderBookFilePath = builder.Configuration["AppSettings:OrderBookFilePath"];
builder.Services.AddPersistence(orderBookFilePath);
builder.Services.AddValidatorsFromAssemblyContaining<OrderInputValidator>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
