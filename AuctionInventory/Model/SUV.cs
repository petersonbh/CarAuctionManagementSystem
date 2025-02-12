namespace AuctionInventory.Model;
public class SUV()
    : Vehicle()
{
    public int NumberOfSeats { get; set; } 

    public override string GetVehicleType()
    {
        return "SUV";
    }
}
