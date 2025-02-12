namespace AuctionInventory.Model;

public class SedanEntity()
    : VehicleEntity()
{
    public override string GetVehicleType()
    {
        return "Sedan";
    }
}
