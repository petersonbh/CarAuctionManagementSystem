
namespace AuctionService;

public class AuctionClosedException : Exception
{
    public AuctionClosedException(string? message) : base(message)
    {
    }

}