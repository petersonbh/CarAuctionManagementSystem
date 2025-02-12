namespace AuctionInventory.CreateVehicle
{
    public class VehicleAlreadyExistsException : Exception
    {
        public VehicleAlreadyExistsException(string message) : base(message) { }
    }
}