namespace WebApi.Models.Search;

public class VisitedLocationSearch : SearchBase
{
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
}