using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using AuctionService.Model;

namespace AuctionService;

public record CreateBidResponse(int Id);

public class CreateBidHandler
{
    private readonly IConnectionFactory _connectionFactory;

    public CreateBidHandler(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CreateBidResponse> Handle(CreateBidCommand command)
    {
        if (!command.IsValid())
        {
            throw new ArgumentException("Invalid command");
        }

        var bid = new Bid
        {
            AuctionId = command.AuctionId,
            BidderId = command.BidderId,
            Amount = command.Amount,
            BidTime = DateTime.Now
        };

        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channell = await connection.CreateChannelAsync();

        await channell.QueueDeclareAsync(queue: "bids", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(bid));
        await channell.BasicPublishAsync(exchange: "", routingKey: "bids", body: body);

        return new CreateBidResponse(bid.Id);
    }
}
