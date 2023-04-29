namespace Database.Models;

public class AnimalType : IModel<long>
{
    public long Id { get; init; }
    public long AnimalId { get; set; }
    public long TypeId { get; set; }
}
