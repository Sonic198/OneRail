using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Orders.Dtos;

namespace server_dotnet.Application.Modules.Orders.Queries;

public record GetOrderByIdQuery(int Id) : IRequest<Response<GetOrderByIdQueryResponse>>
{
}

public record GetOrderByIdQueryResponse : OrderDetailedDto, IObjectResponse;

class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<GetOrderByIdQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetOrderByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Response<GetOrderByIdQueryResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders            
            .Include(n => n.Organization)
            .Include(n => n.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (order is null)
        {
            return new NotFoundResponse(ApplicationMessages.Organization.NotFound);
        }

        var orderDetails = order.Adapt<GetOrderByIdQueryResponse>();
        return orderDetails;
    }
}