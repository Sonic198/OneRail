using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Dtos;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Queries;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Users.Queries;

public record GetUsersQuery : PaginationQueryBase, IRequest<Response<GetUsersQueryResponse>>
{
}

public record GetUsersQueryResponse : PagedCollectionDto<UserDto>, IObjectResponse
{
}

class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Response<GetUsersQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUsersQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = request.PageSize == 0
            ? await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken)
            : await _dbContext.Users.AsNoTracking()
                .OrderByDescending(o => o.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

        var organizationsCount = await _dbContext.Users.AsNoTracking().CountAsync(cancellationToken);

        return new GetUsersQueryResponse
        {
            Items = users.Adapt<IEnumerable<UserDto>>(),
            TotalCount = organizationsCount
        };
    }
}

class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(PaginationQueryBaseValidator paginationQueryBaseValidator)
    {
        Include(paginationQueryBaseValidator);
    }
}