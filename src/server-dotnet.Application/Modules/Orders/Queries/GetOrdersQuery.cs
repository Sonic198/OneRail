using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Dtos;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Queries;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Orders.Dtos;

namespace server_dotnet.Application.Modules.Orders.Queries;

public record GetOrdersQuery : PaginationQueryBase, IRequest<Response<GetOrdersQueryResponse>>
{
}

public record GetOrdersQueryResponse : PagedCollectionDto<OrderDto>, IObjectResponse
{
}

class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Response<GetOrdersQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetOrdersQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetOrdersQueryResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = request.PageSize == 0
            ? await _dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken)
            : await _dbContext.Orders.AsNoTracking()
                .OrderByDescending(o => o.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

        var organizationsCount = await _dbContext.Orders.AsNoTracking().CountAsync(cancellationToken);

        return new GetOrdersQueryResponse
        {
            Items = orders.Adapt<IEnumerable<OrderDto>>(),
            TotalCount = organizationsCount
        };
    }
}

class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator(PaginationQueryBaseValidator paginationQueryBaseValidator)
    {
        Include(paginationQueryBaseValidator);
    }
}
