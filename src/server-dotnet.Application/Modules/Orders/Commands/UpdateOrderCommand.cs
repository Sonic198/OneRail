using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Orders.Commands;

public record UpdateOrderCommand : IRequest<Response<EmptyResponse>>
{
    public int Id { get; init; }
    public decimal TotalAmount { get; init; }
    public int UserId { get; init; }
    public int OrganizationId { get; init; }
}

class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public UpdateOrderCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<EmptyResponse>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (order is null)
        {
            return new NotFoundResponse(ApplicationMessages.Organization.NotFound);
        }

        request.Adapt(order);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new EmptyResponse();
    }
}

class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        RuleFor(n => n.TotalAmount).GreaterThan(0);

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