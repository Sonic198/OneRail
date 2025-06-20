using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Orders.Dtos;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Application.Modules.Orders.Commands;
public record CreateOrderCommand : IRequest<Response<CreateOrderResponse>>
{
    public DateTime OrderDate { get; init; }
    public decimal TotalAmount { get; init; }
    public int UserId { get; init; }
    public int OrganizationId { get; init; }
}

public record CreateOrderResponse : OrderDto, IObjectCreateResponse;

class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreateOrderResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public CreateOrderCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<OrderEntity>();

        _dbContext.Orders.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Adapt<CreateOrderResponse>();
    }
}

class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        RuleFor(n => n.TotalAmount).GreaterThan(0);
        RuleFor(n => n.OrderDate).LessThan(dateTimeProvider.DateTimeUtcNow).WithMessage(ApplicationMessages.DateTime.MustBeInPast);

        RuleFor(n => n.UserId)
            .MustAsync(async (id, cancellationToken) =>
                await dbContext.Users.AnyAsync(u => u.Id == id, cancellationToken))
            .WithMessage(ApplicationMessages.User.NotFound);

        RuleFor(n => n.OrganizationId)
            .MustAsync(async (id, cancellationToken) =>
                await dbContext.Organizations.AnyAsync(o => o.Id == id, cancellationToken))
            .WithMessage(ApplicationMessages.Organization.NotFound);
    }
}
