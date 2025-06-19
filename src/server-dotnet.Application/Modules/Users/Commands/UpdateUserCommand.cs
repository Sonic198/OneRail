using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Users.Commands;

public record UpdateUserCommand : IRequest<Response<EmptyResponse>>
{
    public int Id { get; init; }
    public string? First { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public int OrganizationId { get; init; }
}
