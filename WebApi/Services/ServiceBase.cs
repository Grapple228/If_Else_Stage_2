using Database;

namespace WebApi.Services;

public abstract class ServiceBase
{
    protected readonly IUnitOfWork UnitOfWork;

    protected ServiceBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}