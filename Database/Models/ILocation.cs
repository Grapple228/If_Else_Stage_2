namespace Database.Models;

public interface ILocation
{
    double Latitude { get; set; }
    double Longitude { get; set; }

    public bool IsEqual(ILocation location);
}