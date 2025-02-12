namespace AuctionService
{
    public record CreateAuctionCommand(int VehicleId, DateTime Start, DateTime End, decimal StartingBid, bool IsActive)
    {
        public bool IsValid()
        {
            return Start < End && StartingBid > 0;
        }
    }
}
