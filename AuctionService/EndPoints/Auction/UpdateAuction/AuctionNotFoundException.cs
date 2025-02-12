namespace AuctionService;
public class AuctionNotFoundException: Exception
{
    public AuctionNotFoundException(string message) : base(message) {  }
}
