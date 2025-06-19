using OneOf;
using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public class Response<T0> : OneOfBase<T0, IErrorResponse>
    where T0 : ISuccessResponse
{
    Response(OneOf<T0, IErrorResponse> input) : base(input)
    {
    }

    public static Response<T0> FromT0(T0 input) => new(input);

    public static Response<T0> FromT1(IErrorResponse input) => new(OneOf<T0, IErrorResponse>.FromT1(input));

    public static implicit operator Response<T0>(T0 _) => new(_);

    public static implicit operator Response<T0>(OperationFailedResponse _) => new(_);
    public static implicit operator Response<T0>(NotFoundResponse _) => new(_);
    public static implicit operator Response<T0>(ValidationFailedResponse _) => new(_);
    public static implicit operator Response<T0>(ForbiddenResponse _) => new(_);
}