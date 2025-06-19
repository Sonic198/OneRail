using MediatR;
using server_dotnet.Application.Modules.Users.Commands;
using server_dotnet.Application.Modules.Users.Queries;
using server_dotnet.Interfaces;
using server_dotnet.Responses;

namespace server_dotnet.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var users = app.MapGroup("/api/users").WithTags("Users");

        users.MapGet("/", async (IMediator mediator, HttpContext context, [AsParameters] GetUsersQuery query) =>
        {
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        users.MapGet("/{id:int}", async (IMediator mediator, HttpContext context, int id) =>
        {
            var query = new GetUserByIdQuery(id);
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        users.MapPost("/", async (IMediator mediator, HttpContext context, CreateUserCommand command) =>
        {
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });

        users.MapPut("/{id:int}", async (IMediator mediator, HttpContext context, int id, UpdateUserCommand command) =>
        {
            command = command with { Id = id };
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });


        users.MapDelete("/{id:int}", async (IMediator mediator, HttpContext context, int id) =>
        {
            var command = new DeleteUserCommand(id);
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });
    }
}

