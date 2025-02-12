namespace AuctionInventory.Model;

public abstract class Vehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } 

    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal StartingBid { get; set; }

    public Vehicle() { }

    public abstract string GetVehicleType();
}

