using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Core.EntityFramework;

public record QueryFilter
{
    [Range(1, 501)] public int Size { get; set; } = 20;

    public int Page { get; set; } = 1;
    public bool Desc { get; set; } = true;
}