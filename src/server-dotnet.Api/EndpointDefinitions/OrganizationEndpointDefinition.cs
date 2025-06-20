using MediatR;
using server_dotnet.Application.Modules.Organizations.Commands;
using server_dotnet.Application.Modules.Organizations.Queries;
using server_dotnet.Interfaces;
using server_dotnet.Responses;

namespace server_dotnet.EndpointDefinitions;

public class OrganizationEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var organizations = app.MapGroup("/api/organizations").WithTags("Organizations").RequireAuthorization().RequireRateLimiting("organizationRateLimiter");

        organizations.MapGet("/", async (IMediator mediator, HttpContext context, [AsParameters] GetOrganizationsQuery query) =>
        {
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        organizations.MapGet("/{id:int}", async (IMediator mediator, HttpContext context, [AsParameters] GetOrganizationByIdQuery query) =>
        {
            var response = await mediator.Send(query);
            return response.MatchResponse(context);
        });

        organizations.MapPost("/", async (IMediator mediator, HttpContext context, CreateOrganizationCommand command) =>
        {
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });

        organizations.MapPut("/{id:int}", async (IMediator mediator, HttpContext context, int id, UpdateOrganizationCommand command) =>
        {
            command = command with { Id = id };
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });


        organizations.MapDelete("/{id:int}", async (IMediator mediator, HttpContext context, int id) =>
        {
            var command = new DeleteOrganizationCommand(id);
            var response = await mediator.Send(command);
            return response.MatchResponse(context);
        });
    }
}

