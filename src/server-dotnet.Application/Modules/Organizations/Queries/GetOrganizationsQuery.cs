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
using server_dotnet.Application.Modules.Orders.Dtos;
using server_dotnet.Application.Modules.Organizations.Dtos;

namespace server_dotnet.Application.Modules.Organizations.Queries;

public record GetOrganizationsQuery : PaginationQueryBase, IRequest<Response<GetOrganizationsQueryResponse>>
{
}

public record GetOrganizationsQueryResponse : PagedCollectionDto<OrganizationDto>, IObjectResponse
{
}
