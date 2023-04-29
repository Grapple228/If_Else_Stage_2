using Database.Models;

namespace Database.Repositories;

public class LocationsRepository : RepositoryBase<Location, long>, ILocationsRepository
{
    public LocationsRepository(DatabaseContext context) : base(context)
    {
    }
    
    public Location? GetLocationWithCords(double latitude, double longitude)
    {
        return Get(x => x.Latitude == latitude && x.Longitude == longitude);
    }

}

public interface ILocationsRepository : IRepositoryBase<Location, long>
{
    public Location? GetLocationWithCords(double latitude, double longitude);
}