using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<Response<GetUserByIdQueryResponse>>
{
}

public record GetUserByIdQueryResponse : UserDto, IObjectResponse;

class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<GetUserByIdQueryResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetUserByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return new NotFoundResponse(ApplicationMessages.User.NotFound);
        }

        return user.Adapt<GetUserByIdQueryResponse>();
    }
}