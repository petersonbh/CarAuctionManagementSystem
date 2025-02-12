using AuctionService.Model;

namespace AuctionService;

public record CreateAuctionResult(int Id);
public class CreateAuctionHandler
{
    private readonly AuctionDbContext _dbContext;
    private readonly AuctionManager _auctionManager;

    public CreateAuctionHandler(AuctionDbContext dbContext, AuctionManager auctionManager)
    {
        _dbContext = dbContext;
        _auctionManager = auctionManager;
    }

    public async Task<CreateAuctionResult> Handle(CreateAuctionCommand command)
    {
        if (!command.IsValid())
        {
            throw new ArgumentException("Invalid command");
        }
        var auction = new Auction
        {
            VehicleId = command.VehicleId,
            StartDate = command.Start,
            EndDate = command.End,
            StartingBid = command.StartingBid,
            IsActive = command.IsActive
        };
       
        await _auctionManager.StartAuction(auction);

        // Save auction to database     
        _dbContext.Auctions.Add(auction);
        _dbContext.SaveChanges();

        return new CreateAuctionResult(auction.Id);
    }
}
