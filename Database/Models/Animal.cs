using Database.Enums;

namespace Database.Models;

public class Animal : IModel<long>
{
    private LifeStatus _lifeStatus;

    public Animal()
    {
        LifeStatus = LifeStatus.Alive;
        ChippingDateTime = DateTime.UtcNow;
    }
    
    public long Id { get; init; }
    public virtual ICollection<AnimalType> AnimalTypes { get; set; } = new List<AnimalType>();
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Gender Gender { get; set; }

    public LifeStatus LifeStatus
    {
        get => _lifeStatus;
        set
        {
            _lifeStatus = value;
            DeathDateTime = _lifeStatus switch
            {
                LifeStatus.Alive => null,
                LifeStatus.Dead => DateTime.UtcNow,
                _ => null
            };
        }
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    public DateTime ChippingDateTime { get; private set; }
    public int ChipperId { get; set; }
    public long ChippingLocationId { get; set; }
    public virtual ICollection<VisitedLocation> VisitedLocations { get; set; } = new List<VisitedLocation>();
    public DateTime? DeathDateTime { get; private set; }
}