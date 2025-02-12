namespace AuctionService.Model;

public class Auction
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal StartingBid { get; set; }
    public bool IsActive { get; set; }
    public List<Bid> Bids { get; set; } = new List<Bid>();

    public Auction()
    {
            
    }
   
}
