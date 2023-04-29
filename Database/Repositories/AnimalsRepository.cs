using Database.Models;

namespace Database.Repositories;

public class AnimalsRepository : RepositoryBase<Animal, long>, IAnimalsRepository
{
    public AnimalsRepository(DatabaseContext context) : base(context)
    {
    }

    public override string Includes { get; set; } = "AnimalTypes,VisitedLocations";
    
    public Animal? GetFirstWithType(long typeId)
    {
        return Get(x => x.AnimalTypes.Any(t => t.TypeId == typeId));
    }

    public Animal? GetFirstWithLocation(long locationId)
    {
        return Get(x => x.VisitedLocations.Any(t => t.LocationId == locationId));
    }

    public Animal? GetFirstWithChippingLocation(long locationId)
    {
        return Get(x => x.ChippingLocationId == locationId);
    }

    public Animal? GetFirstWithChipper(long chipperId)
    {
        return Get(x => x.ChipperId == chipperId);
    }
}

public interface IAnimalsRepository : IRepositoryBase<Animal, long>
{
    Animal? GetFirstWithType(long typeId);
    Animal? GetFirstWithLocation(long locationId);
    Animal? GetFirstWithChippingLocation(long locationId);
    Animal? GetFirstWithChipper(long chipperId);
}