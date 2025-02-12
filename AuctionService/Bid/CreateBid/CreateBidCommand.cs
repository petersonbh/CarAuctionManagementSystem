namespace AuctionService
{
    public record CreateBidCommand(int AuctionId, int BidderId, decimal Amount)
    {
        public bool IsValid()
        {
            return Amount > 0;
        }
    }


}
