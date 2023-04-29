using Database;
using Database.Enums;
using Database.Models;

namespace WebApi.Helpers;

public static class DatabaseHelper
{
    public static void ConfigureDatabase(this DatabaseContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        if(context.Accounts.Any())
            return;
        
        context.CreateDefaultAccounts();
    }

    private static void CreateDefaultAccounts(this DatabaseContext context)
    {
        context.Accounts.Add(new Account
        {
            FirstName = "adminFirstName",
            LastName = "adminLastName",
            Email = "admin@simbirsoft.com",
            Password = "qwerty123",
            Role = Role.Admin
        });
        context.Accounts.Add(new Account
        {
            FirstName = "chipperFirstName",
            LastName = "chipperLastName",
            Email = "chipper@simbirsoft.com",
            Password = "qwerty123",
            Role = Role.Chipper
        });
        context.Accounts.Add(new Account
        {
            FirstName = "userFirstName",
            LastName = "userLastName",
            Email = "user@simbirsoft.com",
            Password = "qwerty123",
            Role = Role.User
        });
        context.SaveChanges();
    }
}