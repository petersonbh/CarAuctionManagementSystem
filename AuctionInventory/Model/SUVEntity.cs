namespace AuctionInventory.Model;
public class SUVEntity()
    : VehicleEntity()
{
    public int NumberOfSeats { get; set; } 

    public override string GetVehicleType()
    {
        return "SUV";
    }
}
