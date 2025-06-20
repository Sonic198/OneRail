using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Users.Dtos;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Application.Modules.Users.Commands;

public record CreateUserCommand : IRequest<Response<CreateUserResponse>>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public int OrganizationId { get; init; }
    public DateTime DateCreated { get; init; }
}

public record CreateUserResponse : UserDto, IObjectCreateResponse;

class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<CreateUserResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateUserCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userToCreate = request.Adapt<UserEntity>();

        _dbContext.Users.Add(userToCreate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userToCreate.Adapt<CreateUserResponse>();
    }
}

class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.DateCreated).LessThan(dateTimeProvider.DateTimeUtcNow).WithMessage(ApplicationMessages.DateTime.MustBeInPast);

        RuleFor(x => x.OrganizationId)
            .MustAsync(async (id, cancellationToken) =>
                await dbContext.Organizations.AnyAsync(o => o.Id == id, cancellationToken))
            .WithMessage(ApplicationMessages.Organization.NotFound);
    }
}
