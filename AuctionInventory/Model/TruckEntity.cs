namespace AuctionInventory.Model;

public class TruckEntity() : VehicleEntity()
{
    public int LoadCapacity { get; set; }

    public override string GetVehicleType()
    {
        return "Truck";
    }
}
