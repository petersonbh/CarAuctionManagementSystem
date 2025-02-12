using AuctionService.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService;
public record CreateBidRequest(int AuctionId, int BidderId, decimal Amount);
public record CreateBidResult(int Id);
public static class CreateBidEndPoint
{
    public static void MapCreateBidEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bids", async ([FromBody] CreateBidRequest request, [FromServices] AuctionDbContext dbContext, [FromServices] CreateBidHandler handler) =>
        {
            var command = request.Adapt<CreateBidCommand>();
            var result = await handler.Handle(command);
            var response = result.Adapt<CreateBidResponse>();
            System.Threading.Thread.Sleep(5000);
            return Task.FromResult(Results.Created($"/bids/{response.Id}", response));
        });
    }
}