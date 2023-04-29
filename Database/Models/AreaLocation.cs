namespace Database.Models;

public class AreaLocation : IModel<long>, ILocation
{
    public long Id { get; init; }
    public long AreaId { get; init; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    
    public bool IsEqual(ILocation location)
    {
        return location.Latitude == Latitude && location.Longitude == Longitude;
    }
}