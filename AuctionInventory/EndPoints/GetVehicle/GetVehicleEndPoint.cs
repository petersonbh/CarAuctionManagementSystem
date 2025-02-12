using Microsoft.AspNetCore.Mvc;

namespace AuctionInventory.GetVehicle;

public record GetVehicleResponse(List<VehicleDto> Vehicles);
public record VehicleDto(int Id, string LicensePlate, string Manufacturer, string Model, int Year, decimal StartingBid, string VehicleType);

public static class GetVehicleEndPoint
{
    public static void MapGetVehicleEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/vehicles",
            (string? type, string? manufacturer, string? model, int? year, [FromServices] VehicleContext dbContext, [FromServices] GetVehicleHandler handler) =>
            {
                var query = new GetVehicleQuery(type?.Trim(), manufacturer?.Trim(), model?.Trim(), year.GetValueOrDefault());
                var result = handler.Handle(query);
                var response = new GetVehicleResponse(result.Vehicles.Select(v => new VehicleDto(
                    v.Id,
                    v.LicensePlate,
                    v.Manufacturer,
                    v.Model,
                    v.Year,
                    v.StartingBid,
                    v.GetVehicleType()
                )).ToList());

                return Results.Ok(response);
            });

        app.MapGet("/vehicles/{id}", async (int id, VehicleContext dbContext) =>
        {
            var vehicle = await dbContext.Vehicles.FindAsync(id);
            if (vehicle is null)
            {
                return Results.NotFound();
            }

            var vehicleDto = new VehicleDto(
                vehicle.Id,
                vehicle.LicensePlate,
                vehicle.Manufacturer,
                vehicle.Model,
                vehicle.Year,
                vehicle.StartingBid,
                vehicle.GetVehicleType()
            );

            return Results.Ok(vehicleDto);
        });
    }
}
