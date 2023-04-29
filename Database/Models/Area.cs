namespace Database.Models;

public class Area : IModel<long>
{
    public long Id { get; init; }
    public string Name { get; set; } = null!;
    public virtual ICollection<AreaLocation> AreaLocations { get; set; } = new List<AreaLocation>();
}