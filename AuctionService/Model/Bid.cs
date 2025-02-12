namespace AuctionService.Model;

public class Bid
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    public int BidderId { get; set; } 
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }

    public Bid()
    {
        
    }
    public Bid(int id, int auctionId, int bidderId, decimal amount, DateTime bidTime)
    {
        Id = id;
        AuctionId = auctionId;
        BidderId = bidderId;
        Amount = amount;
        BidTime = bidTime;
    }

 
}
