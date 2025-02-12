using AuctionInventory.Model;

namespace AuctionInventory.GetVehicle;

public record GetVehicleQuery(string type, string manufacturer, string model, int year);

public record GetVehicleResult(List<Vehicle> Vehicles);

public class GetVehicleHandler
{
    private readonly VehicleContext _dbContext;

    public GetVehicleHandler(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GetVehicleResult Handle(GetVehicleQuery query)
    {
        var vehiclesQuery = _dbContext.Vehicles.AsQueryable();

        if (!string.IsNullOrEmpty(query.type))
        {
            vehiclesQuery = query.type switch
            {
                "SUV" => vehiclesQuery.OfType<SUV>(),
                "Truck" => vehiclesQuery.OfType<Truck>(),
                "Sedan" => vehiclesQuery.OfType<Sedan>(),
                "Hatchback" => vehiclesQuery.OfType<Hatchback>(),
                _ => vehiclesQuery
            };
        }

        if (!string.IsNullOrEmpty(query.manufacturer))
        {
            vehiclesQuery = vehiclesQuery.Where(v => v.Manufacturer == query.manufacturer);
        }

        if (!string.IsNullOrEmpty(query.model))
        {
            vehiclesQuery = vehiclesQuery.Where(v => v.Model == query.model);
        }

        if (query.year > 0)
        {
            vehiclesQuery = vehiclesQuery.Where(v => v.Year == query.year);
        }

        var vehicles = vehiclesQuery.ToList();

        return new GetVehicleResult(vehicles);
    }
}
