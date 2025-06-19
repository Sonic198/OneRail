using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public class ArrayResponse<T> : List<T>, IObjectResponse
{
    public ArrayResponse(IEnumerable<T> collection) : base(collection)
    {
    }
}
