namespace CodeSecure.Core.EntityFramework;

public record Page<T>
{
    public required IList<T> Items { get; set; }
    public required long Count { get; set; }
    public required int PageCount { get; set; }
    public required int CurrentPage { get; set; }
    public required int Size { get; set; }
}