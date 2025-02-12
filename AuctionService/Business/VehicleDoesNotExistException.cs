namespace AuctionService
{
    public class VehicleDoesNotExistException : Exception
    {
        public VehicleDoesNotExistException(string message) : base(message) { }
    }
}
