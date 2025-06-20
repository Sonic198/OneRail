using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using static server_dotnet.Application.Common.Messages.ApplicationMessages;

namespace server_dotnet.Application.Modules.Users.Commands;

public record UpdateUserCommand : IRequest<Response<EmptyResponse>>
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public int OrganizationId { get; init; }
}

class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public UpdateUserCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<EmptyResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        if (user == null)
        {
            return new NotFoundResponse(ApplicationMessages.User.NotFound);
        }

        request.Adapt(user);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new EmptyResponse();
    }
}

class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();        
        RuleFor(x => x.OrganizationId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .DependentRules(() =>
            {
                RuleFor(x => x.OrganizationId)
                    .MustAsync(async (id, cancellationToken) =>
                        await dbContext.Organizations.AnyAsync(o => o.Id == id, cancellationToken))
                    .WithMessage(ApplicationMessages.Organization.NotFound);
            });
    }
}