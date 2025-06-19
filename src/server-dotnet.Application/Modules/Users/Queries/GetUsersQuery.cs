using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Dtos;
using server_dotnet.Application.Common.Queries;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Users.Queries;

public record GetUsersQuery : PaginationQueryBase, IRequest<Response<GetUsersQueryResponse>>
{
    public GetUsersQuery()
    {
        
    }
}

public record GetUsersQueryResponse : PagedCollectionDto<UserDto>, IObjectResponse
{
}


