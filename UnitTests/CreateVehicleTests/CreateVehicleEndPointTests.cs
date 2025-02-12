using AuctionInventory;
using AuctionInventory.CreateVehicle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Moq;
using System.Net.Http.Json;

namespace UnitTests.CreateVehicle;

[TestClass]
public class CreateVehicleEndPointTests
{
    [TestMethod]
    public void MapCreateVehicleEndpoint_ShouldReturnCreatedResult()
    {
        // Arrange
        var mockDbContext = new Mock<VehicleContext>(new DbContextOptions<VehicleContext>());
        var mockHandler = new Mock<CreateVehicleHandler>(mockDbContext.Object);

        var mockEndpointRouteBuilder = new Mock<IEndpointRouteBuilder>();
        var mockRequestDelegate = new Mock<RequestDelegate>();

        var createVehicleRequest = new CreateVehicleRequest("Sedan", "ABC1234", "Toyota", "Camry", 2021, 20000, 0, 4);
        var createVehicleResult = new CreateVehicleResult(1);

        var app = new WebApplicationFactory<Program>().CreateClient();

        // Act
        var response = app.PostAsJsonAsync("/vehicles", createVehicleRequest).GetAwaiter().GetResult();

        // Assert
        Assert.AreEqual(StatusCodes.Status201Created, (int)response.StatusCode);
        var responseData = response.Content.ReadFromJsonAsync<CreateVehicleResponse>();
        Assert.IsNotNull(responseData);
        Assert.AreEqual(1, responseData.Id);
    }
}
