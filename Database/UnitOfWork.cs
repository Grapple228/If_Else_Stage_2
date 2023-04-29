using Database.Repositories;

namespace Database;

public interface IUnitOfWork
{
    IAccountsRepository AccountsRepository { get; }
    IAnimalsRepository AnimalsRepository { get; }
    IAreasRepository AreasRepository { get; }
    ITypesRepository TypesRepository { get; }
    ILocationsRepository LocationsRepository { get; }
    IVisitedLocationsRepository VisitedLocationsRepository { get; }
    void Save();
}

public sealed class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly DatabaseContext _context;
    private IAccountsRepository? _accountsRepository;
    private IAnimalsRepository? _animalsRepository;
    private IAreasRepository? _areasRepository;
    private ITypesRepository? _typesRepository;
    private ILocationsRepository? _locationsRepository;
    private IVisitedLocationsRepository? _visitedLocationsRepository;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public IAccountsRepository AccountsRepository =>
        _accountsRepository ??= new AccountsRepository(_context);

    public IAnimalsRepository AnimalsRepository =>
        _animalsRepository ??= new AnimalsRepository(_context);

    public IAreasRepository AreasRepository =>
        _areasRepository ??= new AreasRepository(_context);

    public ITypesRepository TypesRepository =>
        _typesRepository ??= new TypesRepository(_context);

    public ILocationsRepository LocationsRepository =>
        _locationsRepository ??= new LocationsRepository(_context);
    
    public IVisitedLocationsRepository VisitedLocationsRepository =>
        _visitedLocationsRepository ??= new VisitedLocationsRepository(_context);

    public void Save()
    {
        _context.SaveChanges();
    }

    ~UnitOfWork()
    {
       Dispose();  
    }

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}