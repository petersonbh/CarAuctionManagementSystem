using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuctionInventory.CreateVehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuctionInventory.Model;

namespace AuctionInventory.CreateVehicle.Tests;

[TestClass()]
public class CreateVehicleHandlerTests
{
    [TestMethod()]
    public async Task Handle_CreateVehicle_Ok()
    {
        // Arrange
        var mockDbContext = new Mock<VehicleContext>();
        mockDbContext.Setup<DbSet<Vehicle>>(x => x.Vehicles).ReturnsDbSet([]);

        var handler = new CreateVehicleHandler(mockDbContext.Object);

        var command = new CreateVehicleCommand(
            LicensePlate: "ABC123",
            VehicleType: "Sedan",
            Manufacturer: "Toyota",
            Model: "Camry",
            Year: 2021,
            StartingBid: 10000m,
            LoadCapacity: 500,
            NumberOfSeats: 5
        );

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod()]
    [ExpectedException(typeof(VehicleAlreadyExistsException))]
    public async Task Handle_CreateVehicle_AlreadyExists_ThrowsException()
    {
        // Arrange
        var existingVehicle = new Sedan
        {
            LicensePlate = "ABC123",
            Manufacturer = "Toyota",
            Model = "Camry",
            Year = 2021,
            StartingBid = 10000m
        };

        var mockDbContext = new Mock<VehicleContext>();
        mockDbContext.Setup(x => x.Vehicles).ReturnsDbSet(new List<Vehicle> { existingVehicle });

        var handler = new CreateVehicleHandler(mockDbContext.Object);

        var command = new CreateVehicleCommand(
            LicensePlate: "ABC123",
            VehicleType: "Sedan",
            Manufacturer: "Toyota",
            Model: "Camry",
            Year: 2021,
            StartingBid: 10000m,
            LoadCapacity: 500,
            NumberOfSeats: 5
        );

        // Act
        var result = await handler.Handle(command);

        // Assert
        // The ExpectedException attribute will handle the assertion
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidOperationException))]
    public async Task Handle_CreateVehicle_InvalidType_ThrowsException()
    {
        // Arrange
        var mockDbContext = new Mock<VehicleContext>();
        mockDbContext.Setup(x => x.Vehicles).ReturnsDbSet([]);

        var handler = new CreateVehicleHandler(mockDbContext.Object);

        var command = new CreateVehicleCommand(
            LicensePlate: "ABC123",
            VehicleType: "StationWagon",
            Manufacturer: "Toyota",
            Model: "Camry",
            Year: 2021,
            StartingBid: 10000m,
            LoadCapacity: 500,
            NumberOfSeats: 5
        );

        // Act
        var result = await handler.Handle(command);

        // Assert
        // The ExpectedException attribute will handle the assertion
    }
}
