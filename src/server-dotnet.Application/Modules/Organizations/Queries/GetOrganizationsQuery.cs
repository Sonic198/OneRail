using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Dtos;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Queries;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Organizations.Dtos;

namespace server_dotnet.Application.Modules.Organizations.Queries;

public record GetOrganizationsQuery : PaginationQueryBase, IRequest<Response<GetOrganizationsQueryResponse>>
{
}

public record GetOrganizationsQueryResponse : PagedCollectionDto<OrganizationDto>, IObjectResponse
{
}

class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, Response<GetOrganizationsQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetOrganizationsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetOrganizationsQueryResponse>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = request.PageSize == 0
            ? await _dbContext.Organizations.AsNoTracking().ToListAsync(cancellationToken)
            : await _dbContext.Organizations.AsNoTracking()
                .OrderByDescending(o => o.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

        var organizationsCount = await _dbContext.Organizations.AsNoTracking().CountAsync(cancellationToken);

        return new GetOrganizationsQueryResponse
        {
            Items = organizations.Adapt<IEnumerable<OrganizationDto>>(),
            TotalCount = organizationsCount
        };
    }
}

class GetOrganizationsQueryValidator : AbstractValidator<GetOrganizationsQuery>
{
    public GetOrganizationsQueryValidator(PaginationQueryBaseValidator paginationQueryBaseValidator)
    {
        Include(paginationQueryBaseValidator);
    }
}