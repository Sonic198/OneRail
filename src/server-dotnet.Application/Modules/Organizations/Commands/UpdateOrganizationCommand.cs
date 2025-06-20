using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Application.Common.Messages;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Modules.Organizations.Commands;

public record UpdateOrganizationCommand : IRequest<Response<EmptyResponse>>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Industry { get; init; }
    public DateTime DateFounded { get; init; }
}

class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, Response<EmptyResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    public UpdateOrganizationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<EmptyResponse>> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _dbContext.Organizations.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (organization is null)
        {
            return new NotFoundResponse(ApplicationMessages.Organization.NotFound);
        }

        request.Adapt(organization);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new EmptyResponse();
    }
}

class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
{
    public UpdateOrganizationCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        RuleFor(n => n.Name).NotEmpty();
        RuleFor(n => n.DateFounded).LessThan(dateTimeProvider.DateTimeUtcNow).WithMessage(ApplicationMessages.DateTime.MustBeInPast);
    }
}
