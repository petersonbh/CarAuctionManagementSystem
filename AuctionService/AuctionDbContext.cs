using AuctionService.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext()
    {
    }

    public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Auction> Auctions { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Bid>().
            HasKey(b => b.Id);
    }

}
