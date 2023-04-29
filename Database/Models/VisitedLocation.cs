namespace Database.Models;

public class VisitedLocation : IModel<long>
{
    public VisitedLocation()
    {
        DateTimeOfVisitLocationPoint = DateTime.UtcNow;
    }

    public long Id { get; init; }
    public long AnimalId { get; init; }
    public long LocationId { get; set; }
    public DateTime DateTimeOfVisitLocationPoint { get; set; }
}