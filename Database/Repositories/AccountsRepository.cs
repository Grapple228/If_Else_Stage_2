using Database.Models;

namespace Database.Repositories;

public class AccountsRepository : RepositoryBase<Account, int>, IAccountsRepository
{
    public AccountsRepository(DatabaseContext context) : base(context)
    {
    }
    
    public Account? Authenticate(string email, string password)
    {
        return Get(x => x.Email.ToLower() == email && x.Password == password);
    }

    public Account? GetWithEmail(string email)
    {
        return Get(x => x.Email == email);
    }
}

public interface IAccountsRepository : IRepositoryBase<Account, int>
{
    Account? Authenticate(string email, string password);
    Account? GetWithEmail(string email);
}