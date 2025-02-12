using AuctionService.Infrastructure;
using AuctionService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AuctionService;

public class AuctionManager
{
    private readonly AuctionDbContext _dbContext;
    private readonly IVehicleInventoryIntegration _vehicleInventoryIntegration;
    private readonly IMemoryCache _memoryCache;

    public AuctionManager(AuctionDbContext dbContext, IVehicleInventoryIntegration vehicleInventoryIntegration, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _vehicleInventoryIntegration = vehicleInventoryIntegration;
        _memoryCache = memoryCache;
    }

    public async Task StartAuction(Auction auction)
    {
        bool vehicleExists = await _vehicleInventoryIntegration.VehicleExists(auction.VehicleId);
        if (!vehicleExists)
        {
            throw new VehicleDoesNotExistException("Vehicle does not exist");
        }

        bool vehicleHasActiveAuction = _dbContext.Auctions.Any(a => a.VehicleId == auction.VehicleId && a.IsActive);
        if (vehicleHasActiveAuction)
        {
            throw new VehicleHasActiveAuctionException("Vehicle already has an active Auction");
        }

        _dbContext.Auctions.Add(auction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CloseAuction(int auctionId)
    {
        var auction = _dbContext.Auctions.Find(auctionId);
        if (auction == null)
        {
            throw new AuctionNotFoundException("Auction does not exist");
        }
        auction.IsActive = false;
        _dbContext.Auctions.Update(auction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task PlaceBid(Bid bid)
    {
        decimal highestBid = 0;
        if (!_memoryCache.TryGetValue(bid.AuctionId, out highestBid))
        {
            var highestBidQuery = await _dbContext.Bids
            .Where(b => b.AuctionId == bid.AuctionId)
            .Select(b => (decimal?)b.Amount)
            .MaxAsync();

            highestBid = highestBidQuery ?? 0;
            _memoryCache.Set(bid.AuctionId, highestBid);
        }

        if (bid.Amount > highestBid)
        {
            var auction = await _dbContext.Auctions.FindAsync(bid.AuctionId);
            if (auction == null)
            {
                throw new AuctionNotFoundException("Auction does not exist");
            }
            else if (!auction.IsActive)
            {
                throw new AuctionClosedException("Auction is closed");
            }
            auction.Bids.Add(bid);
            await _dbContext.SaveChangesAsync();
            _memoryCache.Set(bid.AuctionId, bid.Amount);
        }
    }
}
