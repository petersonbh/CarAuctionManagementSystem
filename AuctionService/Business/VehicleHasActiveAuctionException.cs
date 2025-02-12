namespace AuctionService;

public class VehicleHasActiveAuctionException : Exception
{
    public VehicleHasActiveAuctionException(string message) : base(message) { }
}
