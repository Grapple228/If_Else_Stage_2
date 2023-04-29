using Database.Models;

namespace Database.Filters;

public class AccountsFilter : IFilter<Account, int>
{
    private readonly string? _email;
    private readonly string? _firstname;
    private readonly string? _lastName;

    public string? Firstname
    {
        get => _firstname;
        init => _firstname = value?.ToLower();
    }

    public string? LastName
    {
        get => _lastName;
        init => _lastName = value?.ToLower();
    }

    public string? Email
    {
        get => _email;
        init => _email = value?.ToLower();
    }

    public IQueryable<Account> Filter(IQueryable<Account> accounts)
    {
        if (!string.IsNullOrEmpty(Firstname))
            accounts = from a in accounts where a.FirstName.ToLower().Contains(Firstname) select a;
        if (!string.IsNullOrEmpty(LastName))
            accounts = from a in accounts where a.LastName.ToLower().Contains(LastName) select a;
        if (!string.IsNullOrEmpty(Email))
            accounts = from a in accounts where a.Email.ToLower().Contains(Email) select a;

        return accounts;
    }
}