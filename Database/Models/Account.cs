using Database.Enums;

namespace Database.Models;

public class Account : IModel<int>
{
    public int Id { get; init; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public Role Role { get; set; }

    public Account()
    {
        Role = Role.User;
    }
}