using AuctionService;
using AuctionService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuctionDbContext>(opt => opt.UseInMemoryDatabase("Auctions"));
builder.Services.AddHttpClient(); 
builder.Services.AddMemoryCache();

// Register RabbitMQ connection
var factory = new ConnectionFactory() { HostName = "localhost" };
builder.Services.AddSingleton(factory.CreateConnectionAsync().GetAwaiter().GetResult());

// Register BidConsumer as a hosted service
// Could be implemented as a worker service to improve the performance of the application
builder.Services.AddHostedService<BidConsumer>();

builder.Services.AddTransient<CreateAuctionHandler>();
builder.Services.AddTransient<CreateBidHandler>();
builder.Services.AddTransient<AuctionManager>();
builder.Services.AddTransient<IVehicleInventoryIntegration, VehicleInventoryIntegration>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AuctionDbContext>();
    context.Database.EnsureCreated();
}

app.MapCreateAuctionEndpoint();
app.MapCreateBidEndpoint();

app.Run();
