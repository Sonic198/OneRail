using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Organizations.Dtos;

namespace server_dotnet.Application.Modules.Organizations.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<Response<GetOrganizationByIdQueryResponse>>
{
}

public record GetOrganizationByIdQueryResponse : OrganizationDto, IObjectResponse;

class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Response<GetOrganizationByIdQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetOrganizationByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetOrganizationByIdQueryResponse>> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await _dbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (organization is null)
        {
            return new NotFoundResponse(ApplicationMessages.Organization.NotFound);
        }

        return organization.Adapt<GetOrganizationByIdQueryResponse>();
    }
}
