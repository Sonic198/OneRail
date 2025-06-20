using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<Response<EmptyResponse>>
{
}

class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteUserCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<EmptyResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (user is null)
        {
            return new NotFoundResponse(ApplicationMessages.User.NotFound);
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EmptyResponse();
    }
}
