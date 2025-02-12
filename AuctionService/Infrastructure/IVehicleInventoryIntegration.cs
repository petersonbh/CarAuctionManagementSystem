namespace AuctionService.Infrastructure
{
    public interface IVehicleInventoryIntegration
    {
        Task<bool> VehicleExists(int vehicleId);
    }
}
