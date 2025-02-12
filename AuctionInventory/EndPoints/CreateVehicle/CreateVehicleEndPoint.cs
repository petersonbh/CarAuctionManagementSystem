using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuctionInventory.CreateVehicle;

public record CreateVehicleRequest(string VehicleType, string LicensePlate, string Manufacturer, string Model, int Year, decimal StartingBid, int LoadCapacity, int NumberOfSeats);
public record CreateVehicleResponse(int Id);
public static class CreateVehicleEndPoint
{
    public static void MapCreateVehicleEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/vehicles", async ([FromBody] CreateVehicleRequest request, [FromServices] VehicleContext dbContext, [FromServices] CreateVehicleHandler handler) =>
        {
            var command = request.Adapt<CreateVehicleCommand>();
            var result = await handler.Handle(command);
            var response = result.Adapt<CreateVehicleResponse>();

            return Task.FromResult(Results.Created($"/vehicles/{response.Id}", response));
        }).AddEndpointFilter<CustomExceptionFilter>();
    }
}
