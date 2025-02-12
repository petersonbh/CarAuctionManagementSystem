namespace AuctionService;

public record UpdateAuctionCommand(int Id, bool IsActive);
public record UpdateAuctionResult(int Id);
public class UpdateAuctionHandler
{
    private readonly AuctionDbContext _dbContext;
    private readonly AuctionManager _auctionManager;

    public UpdateAuctionHandler(AuctionDbContext dbContext, AuctionManager auctionManager)
    {
        _dbContext = dbContext;
        _auctionManager = auctionManager;
    }

    public async Task<UpdateAuctionResult> Handle(UpdateAuctionCommand command)
    {
        if (command.IsActive)
            throw new InvalidOperationException("Cannot update auction to active");

        await _auctionManager.CloseAuction(command.Id);

        return new UpdateAuctionResult(command.Id);
    }
}
