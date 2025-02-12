using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuctionService;
using AuctionService.Model;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuctionService.Tests;

[TestClass]
public class CreateBidHandlerTests
{

    private Mock<IConnectionFactory> _mockConnectionFactory;
    private Mock<IConnection> _mockConnection;
    private Mock<IChannel> _mockChannel;
    private CreateBidHandler _createBidHandler;

    [TestInitialize]
    public void Setup()
    {
        _mockConnectionFactory = new Mock<IConnectionFactory>();
        _mockConnection = new Mock<IConnection>();
        _mockChannel = new Mock<IChannel>();

        _mockConnectionFactory.Setup(x => x.CreateConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockConnection.Object);
        _mockConnection.Setup(x => x.CreateChannelAsync(It.IsAny<CreateChannelOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockChannel.Object);

        _createBidHandler = new CreateBidHandler(_mockConnectionFactory.Object);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task Handle_InvalidCommand_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateBidCommand(1, 1, 0);

        // Act
        await _createBidHandler.Handle(command);

        // Assert is handled by ExpectedException
    }
}
