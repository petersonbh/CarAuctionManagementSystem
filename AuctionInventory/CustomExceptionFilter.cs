using AuctionInventory.CreateVehicle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomExceptionFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (VehicleAlreadyExistsException exception)
        {
            // Handle the exception
            return Results.Problem(
                title: "Vehicle already exists",
                detail: exception.Message,
                statusCode: StatusCodes.Status409Conflict
            );
        }
        catch (Exception exception)
        {
            // Handle the exception
            return Results.Problem(
                title: "An error occurred",
                detail: exception.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is VehicleAlreadyExistsException)
        {
            context.Result = new ConflictObjectResult(new { error = context.Exception.Message });
        }
        else
        {
            context.Result = new ObjectResult(new { error = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        context.ExceptionHandled = true;
    }
}
