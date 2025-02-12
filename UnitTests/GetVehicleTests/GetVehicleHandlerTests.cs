using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuctionInventory.GetVehicle;
using AuctionInventory.Model;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AuctionInventory.GetVehicle.Tests
{
    [TestClass()]
    public class GetVehicleHandlerTests
    {
        private VehicleContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<VehicleContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new VehicleContext(options);
            return context;
        }

        private void SeedData(VehicleContext context)
        {
            context.Vehicles.AddRange(
                new SUVEntity { Id = 1, Manufacturer = "Toyota", LicensePlate = "ABC1234", Model = "RAV4", Year = 2021, StartingBid = 20000m },
                new SedanEntity { Id = 2, Manufacturer = "Honda", LicensePlate = "ABC1255", Model = "Civic", Year = 2020, StartingBid = 15000m },
                new HatchbackEntity { Id = 3, Manufacturer = "KIA", LicensePlate = "DEF1266", Model = "Stonic", Year = 2024, StartingBid = 18000m },
                new SUVEntity { Id = 4, Manufacturer = "Ford", LicensePlate = "GHI1234", Model = "Explorer", Year = 2022, StartingBid = 25000m },
                new SedanEntity { Id = 5, Manufacturer = "Chevrolet", LicensePlate = "JKL1255", Model = "Malibu", Year = 2019, StartingBid = 17000m },
                new HatchbackEntity { Id = 6, Manufacturer = "Volkswagen", LicensePlate = "MNO1266", Model = "Golf", Year = 2023, StartingBid = 19000m },
                new SUVEntity { Id = 7, Manufacturer = "Nissan", LicensePlate = "PQR1234", Model = "Rogue", Year = 2021, StartingBid = 22000m },
                new SedanEntity { Id = 8, Manufacturer = "BMW", LicensePlate = "STU1255", Model = "3 Series", Year = 2020, StartingBid = 30000m },
                new HatchbackEntity { Id = 9, Manufacturer = "Hyundai", LicensePlate = "VWX1266", Model = "i30", Year = 2022, StartingBid = 16000m },
                new SUVEntity { Id = 10, Manufacturer = "Mazda", LicensePlate = "YZA1234", Model = "CX-5", Year = 2021, StartingBid = 24000m }
            );
            context.SaveChanges();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var context = CreateInMemoryDbContext();
            context.Database.EnsureDeleted();
            SeedData(context);
        }

        [TestMethod()]
        public void Handle_GetVehiclesByType_Ok()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var handler = new GetVehicleHandler(context);
            var query = new GetVehicleQuery(type: "SUV", manufacturer: null, model: null, year: 0);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Vehicles.Count);
            Assert.IsTrue(result.Vehicles.All(v => v.GetVehicleType() == "SUV"));
        }

        [TestMethod()]
        public void Handle_GetVehiclesByManufacturer_Ok()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var handler = new GetVehicleHandler(context);
            var query = new GetVehicleQuery(type: null, manufacturer: "Toyota", model: null, year: 0);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Vehicles.Count);
            Assert.IsTrue(result.Vehicles.All(v => v.Manufacturer == "Toyota"));
        }

        [TestMethod()]
        public void Handle_GetVehiclesByModel_Ok()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var handler = new GetVehicleHandler(context);
            var query = new GetVehicleQuery(type: null, manufacturer: null, model: "Stonic", year: 0);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Vehicles.Count);
            Assert.AreEqual("Stonic", result.Vehicles.First().Model);
        }

        [TestMethod()]
        public void Handle_GetVehiclesByYear_Ok()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var handler = new GetVehicleHandler(context);
            var query = new GetVehicleQuery(type: null, manufacturer: null, model: null, year: 2021);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Vehicles.Count);
            Assert.IsTrue(result.Vehicles.All(v => v.Year == 2021));
        }

        [TestMethod()]
        public void Handle_GetVehiclesByAllCriteria_Ok()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var handler = new GetVehicleHandler(context);
            var query = new GetVehicleQuery(type: "SUV", manufacturer: "Toyota", model: "RAV4", year: 2021);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Vehicles.Count);
            Assert.IsTrue(result.Vehicles.All(v => v.Manufacturer == "Toyota" && v.Model == "RAV4" && v.Year == 2021));
        }
    }
}
