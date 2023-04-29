using Database.Models;

namespace Database.Filters;

public class VisitedLocationFilter : IFilter<VisitedLocation, long>
{
    public long? AnimalId { get; init; }
    public DateTime? StartDateTime { get; init; }
    public DateTime? EndDateTime { get; set; }

    public IQueryable<VisitedLocation> Filter(IQueryable<VisitedLocation> models)
    {
        if (AnimalId != null)
            models = from m in models where m.AnimalId == AnimalId select m;

        if(StartDateTime != null)
            models = from m in models where m.DateTimeOfVisitLocationPoint >= StartDateTime.Value.ToUniversalTime() select m;

        if (EndDateTime == null) return models;
        
        // Нужно добавить к дате 1 секунду, иначе в фильтр не попадаются даты, которые равны дате
        EndDateTime = EndDateTime.Value.AddSeconds(1);
        models = from m in models where m.DateTimeOfVisitLocationPoint <= EndDateTime.Value.ToUniversalTime() select m;

        return models;
    }
}