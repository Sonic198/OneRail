using FluentValidation;

namespace server_dotnet.Application.Common.Queries;
public record PaginationQueryBase
{    
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}

public class PaginationQueryBaseValidator : AbstractValidator<PaginationQueryBase>
{
    public PaginationQueryBaseValidator()
    {
        RuleFor(n => n.PageSize).GreaterThanOrEqualTo(0);
        RuleFor(n => n.PageNumber).GreaterThanOrEqualTo(1);
    }
}