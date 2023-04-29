using Database.Models;

namespace Database.Filters;

public interface IFilter<TModel, TId> 
    where TModel : IModel<TId> 
    where TId : struct
{
    public IQueryable<TModel> Filter(IQueryable<TModel> models);
}