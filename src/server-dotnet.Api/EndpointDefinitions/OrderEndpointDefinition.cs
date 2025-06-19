using MediatR;
using server_dotnet.Application.Modules.Orders.Commands;
using server_dotnet.Application.Modules.Orders.Queries;
using server_dotnet.Interfaces;
using server_dotnet.Responses;

namespace server_dotnet.EndpointDefinitions;

public class OrderEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var orders = app.MapGroup("/api/orders").WithTags("Orders");

        orders.MapGet("/", async (IMediator mediator, HttpContext context, [AsParameters] GetOrdersQuery query) =>
        {
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        orders.MapGet("/{id:int}", async (IMediator mediator, HttpContext context, int id) =>
        {
            var query = new GetOrderByIdQuery(id);
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        orders.MapPost("/", async (IMediator mediator, HttpContext context, CreateOrderCommand command) =>
        {
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });

        orders.MapPut("/{id:int}", async (IMediator mediator, HttpContext context, int id, UpdateOrderCommand command) =>
        {
            command = command with { Id = id };
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });


        orders.MapDelete("/{id:int}", async (IMediator mediator, HttpContext context, int id) =>
        {
            var command = new DeleteOrderCommand(id);
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });
    }
}
