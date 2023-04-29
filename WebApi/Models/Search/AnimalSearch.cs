using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace WebApi.Models.Search;

public class AnimalSearch : SearchBase
{
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    [Range(1, int.MaxValue)] public int? ChipperId { get; set; }
    [Range(1, long.MaxValue)] public long? ChippingLocationId { get; set; }
    public LifeStatus? LifeStatus { get; set; }
    public Gender? Gender { get; set; }
}