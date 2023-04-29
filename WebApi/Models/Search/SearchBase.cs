using System.ComponentModel.DataAnnotations;
using WebApi.Misc;

namespace WebApi.Models.Search;

public abstract class SearchBase
{
    [Range(0, int.MaxValue)]
    public int From { get; set; } = DefaultSearchParameters.From;
    [Range(1, int.MaxValue)]
    public int Size { get; set; } = DefaultSearchParameters.Size;
}