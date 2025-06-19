using FluentValidation;
using MediatR;
using OneOf;
using server_dotnet.Application.Common.Extensions;
using server_dotnet.Application.Common.Responses;

namespace server_dotnet.Application.Common.Behaviors;

class ValidationBehavior<TRequest, TResponse> : BehaviorBase<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(n => n.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
            {
                return GetErrorResponse(new ValidationFailedResponse(failures.GetErrors()));
            }
        }
        return await next();
    }
}
