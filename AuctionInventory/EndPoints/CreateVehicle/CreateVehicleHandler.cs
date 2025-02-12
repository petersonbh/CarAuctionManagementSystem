using AuctionInventory.Model;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CreateVehicleResult> Handle(CreateVehicleCommand command)
    {
        Vehicle? vehicle = await _dbContext.Vehicles.SingleOrDefaultAsync(v => v.LicensePlate == command.LicensePlate);
        if (vehicle != null)
        {
            throw new VehicleAlreadyExistsException("Vehicle already exists.");
        }

        switch (command.VehicleType)
        {
            case "Sedan":
                vehicle = new Sedan { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid };
                break;
            case "Truck":
                vehicle = new Truck { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid, LoadCapacity = command.LoadCapacity };
                break;
            case "Hatchback":
                vehicle = new Hatchback { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid };
                break;
            case "SUV":
                vehicle = new SUV { LicensePlate = command.LicensePlate, Manufacturer = command.Manufacturer, Model = command.Model, Year = command.Year, StartingBid = command.StartingBid, NumberOfSeats = command.NumberOfSeats };
                break;
        }

        await _dbContext.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        if (vehicle == null)
        {
            throw new InvalidOperationException("Vehicle creation failed.");
        }
        return new CreateVehicleResult(vehicle.Id);
    }
}

