using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using server_dotnet.Application.Common.Responses;
using server_dotnet.Application.Common.Responses.Interfaces;
using server_dotnet.Extensions;
using server_dotnet.Services.Interfaces;

namespace server_dotnet.Responses;

public static class ApiResponseExtension
{
    public static IResult MatchResponse<T1>(
        this Response<T1> oneOf,
        HttpContext httpContext)
        where T1 : ISuccessResponse
        => MatchPotentialResponse(oneOf.Value, httpContext, httpContext.RequestServices.GetRequiredService<IActivityProvider>().Current);

    private static IResult MatchPotentialResponse<TResponse>(
        TResponse response,
        HttpContext httpContext,
        Activity activity)
        => response switch
        {
            IObjectResponse r => Results.Ok(r),
            IObjectCreateResponse r => Results.Created($"{httpContext.Request.Path}/{HttpUtility.UrlEncode(r.Id.ToString(), Encoding.UTF8)}", r),
            EmptyResponse => Results.NoContent(),            
            ForbiddenResponse r => Results.Json(CreateApiErrorResponse(HttpStatusCode.Forbidden, activity.GetCurrentTraceIdentifier(), data: r.Errors), statusCode: StatusCodes.Status403Forbidden),
            ValidationFailedResponse r => Results.UnprocessableEntity(CreateApiErrorResponse(HttpStatusCode.UnprocessableEntity, activity.GetCurrentTraceIdentifier(), data: r.Errors)),
            OperationFailedResponse r => Results.BadRequest(CreateApiErrorResponse(HttpStatusCode.BadRequest, activity.GetCurrentTraceIdentifier(), r.Message, r.Errors)),
            NotFoundResponse r => Results.NotFound(CreateApiErrorResponse(HttpStatusCode.NotFound, activity.GetCurrentTraceIdentifier(), r.Message)),
            _ => Results.BadRequest(CreateApiErrorResponse(HttpStatusCode.BadRequest, activity.GetCurrentTraceIdentifier(), "Server can't process the request."))
        };

    private static ApiErrorResponse CreateApiErrorResponse(HttpStatusCode statusCode, string traceId, string message = null, object data = null)
        => new(statusCode, traceId, message, data);
}