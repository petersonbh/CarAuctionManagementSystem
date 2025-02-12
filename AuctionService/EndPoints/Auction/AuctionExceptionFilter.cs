namespace AuctionService
{
    public class AuctionExceptionFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                return await next(context);
            }
            catch (VehicleDoesNotExistException exception)
            {
                return Results.Problem(
                    title: "Vehicle has not been registered",
                    detail: exception.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (VehicleHasActiveAuctionException exception)
            {
                return Results.Problem(
                    title: "Vehicle already has an active auction",
                    detail: exception.Message,
                    statusCode: StatusCodes.Status409Conflict
                );
            }
            catch(AuctionNotFoundException exception)
            {
                return Results.Problem(
                    title: "Auction not found",
                    detail: exception.Message,
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            catch (AuctionClosedException)
            {
                return Results.Problem(
                    title: "Auction is closed",
                    detail: "The auction has already been closed",
                    statusCode: StatusCodes.Status409Conflict
                );
            }
            catch (ArgumentException exception)
            {
                return Results.Problem(
                    title: "Invalid command",
                    detail: exception.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            catch (Exception exception)
            {
                return Results.Problem(
                    title: "An error occurred",
                    detail: exception.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
