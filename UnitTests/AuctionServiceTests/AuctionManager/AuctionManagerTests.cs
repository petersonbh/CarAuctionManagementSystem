using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuctionService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using AuctionService.Infrastructure;

namespace AuctionService.Tests
{
    [TestClass]
    public class AuctionManagerTests
    {
        private AuctionDbContext _dbContext;
        private Mock<IVehicleInventoryIntegration> _mockVehicleInventoryIntegration;
        private IMemoryCache _memoryCache;
        private AuctionManager _auctionManager;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
                .UseInMemoryDatabase(databaseName: "AuctionDb")
                .Options;
            _dbContext = new AuctionDbContext(options);
            _mockVehicleInventoryIntegration = new Mock<IVehicleInventoryIntegration>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _auctionManager = new AuctionManager(_dbContext, _mockVehicleInventoryIntegration.Object, _memoryCache);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Auctions.AddRange(
                new Auction { Id = 1, VehicleId = 1, IsActive = true },
                new Auction { Id = 2, VehicleId = 2, IsActive = false }
            );
            _dbContext.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(VehicleDoesNotExistException))]
        public async Task StartAuction_VehicleDoesNotExist_ThrowsException()
        {
            // Arrange
            var auction = new Auction { VehicleId = 3 };
            _mockVehicleInventoryIntegration.Setup(x => x.VehicleExists(It.IsAny<int>()))
                .ReturnsAsync(false);

            await _auctionManager.StartAuction(auction);
        }

        [TestMethod]
        [ExpectedException(typeof(VehicleHasActiveAuctionException))]
        public async Task StartAuction_VehicleHasActiveAuction_ThrowsException()
        {
            // Arrange
            var auction = new Auction { VehicleId = 1 };
            _mockVehicleInventoryIntegration.Setup(x => x.VehicleExists(It.IsAny<int>()))
                .ReturnsAsync(true);

            await _auctionManager.StartAuction(auction);            
        }

        [TestMethod]
        public async Task StartAuction_ValidAuction_AddsAuction()
        {
            // Arrange
            var auction = new Auction { VehicleId = 3 };
            _mockVehicleInventoryIntegration.Setup(x => x.VehicleExists(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _auctionManager.StartAuction(auction);

            // Assert
            Assert.AreEqual(3, _dbContext.Auctions.Count());
            Assert.IsTrue(_dbContext.Auctions.Any(a => a.VehicleId == 3));
        }

        [TestMethod]
        [ExpectedException(typeof(AuctionNotFoundException))]
        public async Task CloseAuction_AuctionDoesNotExist_ThrowsException()
        {
            // Arrange, Act and Assert
            await _auctionManager.CloseAuction(3);
        }

        [TestMethod]
        public async Task CloseAuction_ValidAuction_ClosesAuction()
        {
            // Arrange
            var auction = _dbContext.Auctions.First(a => a.Id == 1);

            // Act
            await _auctionManager.CloseAuction(1);

            // Assert
            Assert.IsFalse(auction.IsActive);
        }

        [TestMethod]
        public async Task PlaceBid_HigherThanCurrentBid_AddsBid()
        {
            // Arrange
            var auction = _dbContext.Auctions.First(a => a.Id == 1);
            var bid = new Bid { AuctionId = 1, Amount = 200 };

            // Act
            await _auctionManager.PlaceBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(200, auction.Bids.First().Amount);

        }

        [TestMethod]
        public async Task PlaceBid_LowerThanCurrentBid_DoesNotAddBid()
        {
            // Arrange
            var auction = _dbContext.Auctions.First(a => a.Id == 1);
            auction.Bids.Add(new Bid { AuctionId = 1, Amount = 100 });
            _dbContext.SaveChanges();

            var bid = new Bid { AuctionId = 1, Amount = 50 };

            // Act
            await _auctionManager.PlaceBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(100, auction.Bids.First().Amount);
        }

        [TestMethod]
        [ExpectedException(typeof(AuctionClosedException))]
        public async Task PlaceBid_InactiveAuction_ThrowsException()
        {
            // Arrange
            var auction = _dbContext.Auctions.First(a => a.Id == 2);
            var bid = new Bid { AuctionId = 2, Amount = 200 };

            // Act
            await _auctionManager.PlaceBid(bid);
            Assert.AreEqual(0, auction.Bids.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(AuctionNotFoundException))]
        public async Task PlaceBid_InvalidAuction_ThrowsException()
        {
            // Arrange
            var bid = new Bid { AuctionId = 3, Amount = 200 };

            // Act
            await _auctionManager.PlaceBid(bid);
        }
    }
}
