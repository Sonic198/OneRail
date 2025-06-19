using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<Response<GetUserByIdQueryResponse>>
{
}

public record GetUserByIdQueryResponse : UserDto, IObjectResponse;