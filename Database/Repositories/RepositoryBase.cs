using System.Linq.Expressions;
using Database.Filters;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public interface IRepositoryBase<TModel, TId> where TModel : class, IModel<TId> where TId : struct
{
    public IQueryable<TModel> GetAll(Expression<Func<TModel, bool>>? filter = null);
    public IQueryable<TModel> GetAll(int size, int from, Expression<Func<TModel, bool>>? filter = null);
    public IQueryable<TModel> GetAll(IFilter<TModel, TId> filter);
    TModel? Get(TId id);
    public TModel? Get(Expression<Func<TModel, bool>> filter);
    void Create(TModel entity);
    void Delete(TId id);
    void Delete(TModel entityToDelete);
    void Update(TModel entityToUpdate);
    public string Includes { get; set; }
}

public abstract class RepositoryBase<TModel, TId> : 
    IRepositoryBase<TModel, TId> where TModel : class, IModel<TId> where TId : struct
{
    protected readonly DatabaseContext Context;
    private DbSet<TModel> DbSet { get; }

    public virtual string Includes { get; set; } = "";

    protected RepositoryBase(DatabaseContext context)
    {
        Context = context;
        DbSet = context.Set<TModel>();
    }

    private IQueryable<TModel> GetIncludes(IQueryable<TModel> query)
    {
        foreach (var includeProperty in Includes.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query;
    }
    
    public virtual IQueryable<TModel> GetAll(Expression<Func<TModel, bool>>? filter = null)
    {
        IQueryable<TModel> query = DbSet;

        if (filter != null)
            query = query.Where(filter);
        
        return GetIncludes(query);
    }

    public virtual IQueryable<TModel> GetAll(int size, int from, Expression<Func<TModel, bool>>? filter = null)
    {
        return GetAll(filter).Skip(from).Take(size);
    }

    public IQueryable<TModel> GetAll(IFilter<TModel, TId> filter)
    {
        return filter.Filter(GetIncludes(DbSet));
    }

    public virtual TModel? Get(TId id)
    {
        return GetIncludes(DbSet).FirstOrDefault(x => x.Id.Equals(id));
    }
    public virtual TModel? Get(Expression<Func<TModel, bool>> filter)
    {
        return GetIncludes(DbSet).FirstOrDefault(filter);
    }

    public virtual void Create(TModel entity)
    {
        DbSet.Add(entity);
    }

    public virtual void Delete(TId id)
    {
        var entityToDelete = DbSet.Find(id);
        if(entityToDelete == null) return; 
        Delete(entityToDelete);
    }

    public virtual void Delete(TModel entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
        {
            DbSet.Attach(entityToDelete);
        }
        DbSet.Remove(entityToDelete);
    }

    public virtual void Update(TModel entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}