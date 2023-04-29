using Database.Enums;
using Database.Models;

namespace Database.Filters;

public class AnimalFilter : IFilter<Animal, long>
{
    public DateTime? StartDateTime { get; init; }
    public DateTime? EndDateTime { get; set; }
    public int? ChipperId { get; init; }
    public long? ChippingLocationId { get; init; }
    public LifeStatus? LifeStatus { get; init; }
    public Gender? Gender { get; init; }

    public IQueryable<Animal> Filter(IQueryable<Animal> models)
    {
        if (StartDateTime != null)
            models = from m in models where m.ChippingDateTime >= StartDateTime select m;
        if (EndDateTime != null)
        {
            EndDateTime = EndDateTime.Value.AddSeconds(1);
            models = from m in models where m.ChippingDateTime <= EndDateTime select m;
        }
        if (ChipperId != null)
            models = from m in models where m.ChipperId == ChipperId select m;
        if (ChippingLocationId != null)
            models = from m in models where m.ChippingLocationId == ChippingLocationId select m;
        if (LifeStatus != null)
            models = from m in models where m.LifeStatus == LifeStatus select m;
        if (Gender != null)
            models = from m in models where m.Gender == Gender select m;

        return models;
    }
}