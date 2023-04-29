using Database.Models;

namespace Database.Repositories;

public class TypesRepository : RepositoryBase<AType, long>, ITypesRepository
{
    public TypesRepository(DatabaseContext context) : base(context)
    {
    }

}

public interface ITypesRepository : IRepositoryBase<AType, long>
{
    public AType? GetWithValue(string type)
    {
        return Get(x => x.Type == type);
    }
}