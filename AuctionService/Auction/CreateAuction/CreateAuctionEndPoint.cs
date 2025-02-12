using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService;

public record CreateAuctionRequest(int VehicleId, DateTime Start, DateTime End, decimal StartingBid, bool IsActive );
public record CreateAuctionResponse(int Id);
public static class CreateAuctionEndPoint
{
    public static void MapCreateAuctionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auctions", ([FromBody] CreateAuctionRequest request, [FromServices] AuctionDbContext dbContext, [FromServices] CreateAuctionHandler handler) =>
        {
            var command = request.Adapt<CreateAuctionCommand>();
            var result = handler.Handle(command);
            var response = result.Adapt<CreateAuctionResponse>();

            // Save vehicle to database
            return Task.FromResult(Results.Created($"/vehicles/{response.Id}", response));

        }).AddEndpointFilter<AuctionExceptionFilter>();
    }
}
