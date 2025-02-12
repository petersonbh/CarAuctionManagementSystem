namespace AuctionInventory.Model;

public class Truck() : Vehicle()
{
    public int LoadCapacity { get; set; }

    public override string GetVehicleType()
    {
        return "Truck";
    }
}
