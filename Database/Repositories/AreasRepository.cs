using Database.Models;

namespace Database.Repositories;

public class AreasRepository : RepositoryBase<Area, long>, IAreasRepository
{
    public AreasRepository(DatabaseContext context) : base(context)
    {
    }

    public override string Includes { get; set; } = "AreaLocations";
    public Area? GetWithName(string name)
    {
        return Get(x => x.Name == name);
    }
}

public interface IAreasRepository : IRepositoryBase<Area, long>
{
    public Area? GetWithName(string name);
}