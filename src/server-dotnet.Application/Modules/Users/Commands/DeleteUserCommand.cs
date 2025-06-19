using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<Response<EmptyResponse>>
{
}
