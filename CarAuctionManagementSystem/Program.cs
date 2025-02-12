using AuctionInventory;
using AuctionInventory.CreateVehicle;
using AuctionInventory.GetVehicle;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<CreateVehicleHandler>();
builder.Services.AddTransient<GetVehicleHandler>();
builder.Services.AddDbContext<VehicleContext>(opt => opt.UseInMemoryDatabase("Vehicles"));

var app = builder.Build();

// Ensure the database is created and seeded with data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<VehicleContext>();
    context.Database.EnsureCreated();
}

app.MapCreateVehicleEndpoint();
app.MapGetVehicleEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

