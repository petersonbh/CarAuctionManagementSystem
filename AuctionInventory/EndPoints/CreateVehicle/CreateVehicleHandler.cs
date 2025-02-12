using AuctionInventory.Model;

namespace AuctionInventory.CreateVehicle;

public record CreateVehicleCommand(string VehicleType, string LicensePlate, string Manufacturer, string Model, int Year, decimal StartingBid, int LoadCapacity, int NumberOfSeats);

public record CreateVehicleResult(int Id);

public class CreateVehicleHandler
{
    private readonly VehicleContext _dbContext;

    public CreateVehicleHandler(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public CreateVehicleResult Handle(CreateVehicleCommand command)
    {
        VehicleEntity? vehicle = _dbContext.Vehicles.SingleOrDefault(v => v.LicensePlate == command.LicensePlate);
        if (vehicle != null)
        {
            throw new VehicleAlreadyExistsException("Vehicle already exists.");
        }

        switch (command.VehicleType)
        {
            case "Sedan":
                vehicle = new SedanEntity { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid };
                break;
            case "Truck":
                vehicle = new TruckEntity { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid, LoadCapacity = command.LoadCapacity };
                break;
            case "Hatchback":
                vehicle = new HatchbackEntity { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid };
                break;
            case "SUV":
                vehicle = new SUVEntity { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid, NumberOfSeats = command.NumberOfSeats };
                break;
        }

        // Save vehicle to database
        _dbContext.Add(vehicle);
        _dbContext.SaveChanges();

        if (vehicle == null)
        {
            throw new InvalidOperationException("Vehicle creation failed.");
        }
        return new CreateVehicleResult(vehicle.Id);
    }
}
