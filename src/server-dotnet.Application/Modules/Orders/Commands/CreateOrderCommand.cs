using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Orders.Dtos;
using server_dotnet.Application.Modules.Users.Commands;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Orders.Commands;
public record CreateOrderCommand : IRequest<Response<CreateOrderResponse>>
{
}

public record CreateOrderResponse : OrderDto, IObjectCreateResponse;
