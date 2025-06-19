using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Organizations.Dtos;
using server_dotnet.Application.Modules.Users.Queries;

namespace server_dotnet.Application.Modules.Organizations.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<Response<GetOrganizationByIdQueryResponse>>
{
}

public record GetOrganizationByIdQueryResponse : OrganizationDto, IObjectResponse;