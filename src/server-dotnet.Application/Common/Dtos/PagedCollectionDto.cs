namespace server_dotnet.Application.Common.Dtos;
public record PagedCollectionDto<T>
{
    public PagedCollectionDto()
    {
    }

    public PagedCollectionDto(IEnumerable<T> items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public IEnumerable<T>? Items { get; init; }
    public long TotalCount { get; init; }
}
