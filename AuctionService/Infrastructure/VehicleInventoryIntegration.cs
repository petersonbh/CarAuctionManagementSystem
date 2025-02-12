namespace AuctionService.Infrastructure
{
    public class VehicleInventoryIntegration : IVehicleInventoryIntegration
    {
        private readonly HttpClient _httpClient;

        public VehicleInventoryIntegration(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VehicleExists(int vehicleId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7091/vehicles/{vehicleId}");
            return response.IsSuccessStatusCode;
        }
    }
}
