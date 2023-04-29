namespace Database.Models;

public class Location : IModel<long>, ILocation
{
    public Location()
    {
        
    }

    public Location(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
    
    public long Id { get; init; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public bool IsEqual(ILocation location)
    {
        return location.Latitude == Latitude && location.Longitude == Longitude;
    }
}