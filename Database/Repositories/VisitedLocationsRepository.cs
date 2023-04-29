using Database.Models;

namespace Database.Repositories;

public class VisitedLocationsRepository : RepositoryBase<VisitedLocation, long>, IVisitedLocationsRepository
{
    public VisitedLocationsRepository(DatabaseContext context) : base(context)
    {
    }
}

public interface IVisitedLocationsRepository : IRepositoryBase<VisitedLocation, long>
{
}