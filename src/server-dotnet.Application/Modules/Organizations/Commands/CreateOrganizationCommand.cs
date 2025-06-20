using FluentValidation;
using Mapster;
using MediatR;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Application.Modules.Organizations.Dtos;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Application.Modules.Organizations.Commands;

public record CreateOrganizationCommand : IRequest<Response<CreateOrganizationResponse>>
{
    public string? Name { get; init; }
    public string? Industry { get; init; }
    public DateTime DateFounded { get; init; }
}

public record CreateOrganizationResponse : OrganizationDto, IObjectCreateResponse;

class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Response<CreateOrganizationResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateOrganizationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<CreateOrganizationResponse>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<OrganizationEntity>();

        _dbContext.Organizations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Adapt<CreateOrganizationResponse>();
    }
}

class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        RuleFor(n => n.Name).NotEmpty();
        RuleFor(n => n.DateFounded).LessThan(dateTimeProvider.DateTimeUtcNow).WithMessage(ApplicationMessages.DateTime.MustBeInPast);
    }
}