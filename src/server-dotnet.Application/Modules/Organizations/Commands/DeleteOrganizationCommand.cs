using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Organizations.Commands;

public record DeleteOrganizationCommand(int Id) : IRequest<Response<EmptyResponse>>
{
}

class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteOrganizationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<EmptyResponse>> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _dbContext.Organizations.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (organization is null)
        {
            return new NotFoundResponse(ApplicationMessages.Organization.NotFound);
        }

        _dbContext.Organizations.Remove(organization);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EmptyResponse();
    }
}
