using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Metadata;
using AuctionService.Model;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuctionService;

public class BidConsumer : BackgroundService
{
    // This class could be implemented as a worker service to improve the performance of the application

    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public BidConsumer(IConnection connection, IServiceScopeFactory serviceScopeFactory)
    {
        _connection = connection;
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclareAsync(queue: "bids", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var bid = JsonSerializer.Deserialize<Bid>(message);
            if (bid == null)
            {
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                // we need a distributed lock if we want to add more consumers
                var auctionManager = scope.ServiceProvider.GetRequiredService<AuctionManager>();
                await auctionManager.PlaceBid(bid);
            }
        };

         _channel.BasicConsumeAsync(queue: "bids", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
        base.Dispose();
    }
}


