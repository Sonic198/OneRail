using System.Collections.Concurrent;
using MediatR;
using OneOf;
using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Behaviors;

public abstract class BehaviorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private static readonly ConcurrentDictionary<Type, Delegate> _delegateDictionary = new();

    public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

    protected static TResponse GetErrorResponse(IErrorResponse response)
    {
        var responseType = typeof(TResponse);
        if (_delegateDictionary.TryGetValue(responseType, out var @delegate))
        {
            return ((Func<IErrorResponse, TResponse>)@delegate)(response);
        }

        var newDelegate = CreateDelegate(responseType);
        return ((Func<IErrorResponse, TResponse>)newDelegate)(response);
    }

    private static Delegate CreateDelegate(Type type)
    {
        var method = type.GetMethod("FromT1");
        var @delegate = method!.CreateDelegate(typeof(Func<IErrorResponse, TResponse>));

        _delegateDictionary.TryAdd(type, @delegate);

        return @delegate;
    }
}
