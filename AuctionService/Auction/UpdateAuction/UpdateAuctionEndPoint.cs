using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService;
public record UpdateAuctionRequest(int VehicleId, DateTime Start, DateTime End, decimal StartingBid, bool IsActive);
public record UpdateAuctionResponse(int Id);
public static class UpdateAuctionEndPoint
{
    public static void MapUpdateAuctionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/auctions", ([FromBody] UpdateAuctionRequest request, [FromServices] AuctionDbContext dbContext, [FromServices] UpdateAuctionHandler handler) =>
        {
            var command = request.Adapt<UpdateAuctionCommand>();
            var result = handler.Handle(command);
            var response = result.Adapt<UpdateAuctionResponse>();

            return Task.FromResult(Results.NoContent());

        }).AddEndpointFilter<AuctionExceptionFilter>();
    }
}
