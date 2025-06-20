using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Orders.Commands;

public record DeleteOrderCommand(int Id) : IRequest<Response<EmptyResponse>>
{
}

class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public DeleteOrderCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<EmptyResponse>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (order is null)
        {
            return new NotFoundResponse(ApplicationMessages.Order.NotFound);
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EmptyResponse();
    }
}
