using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Orders.Commands;
using server_dotnet.Application.Modules.Orders.Dtos;
using server_dotnet.Application.Modules.Organizations.Dtos;

namespace server_dotnet.Application.Modules.Organizations.Commands;

public record CreateOrganizationCommand : IRequest<Response<CreateOrganizationResponse>>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Industry { get; init; }
    public DateTime DateFounded { get; init; }
}

public record CreateOrganizationResponse : OrganizationDto, IObjectCreateResponse;