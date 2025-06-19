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
using server_dotnet.Application.Modules.Users.Dtos;
using server_dotnet.Application.Modules.Users.Queries;

namespace server_dotnet.Application.Modules.Orders.Queries;

public record GetOrdersQuery : PaginationQueryBase, IRequest<Response<GetOrdersQueryResponse>>
{
}

public record GetOrdersQueryResponse : PagedCollectionDto<OrderDto>, IObjectResponse
{
}
