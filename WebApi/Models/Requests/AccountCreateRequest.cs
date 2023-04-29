using Database.Enums;
using WebApi.Attributes;

namespace WebApi.Requests;

public record AccountCreateRequest(
    [NotEmptyString]string FirstName, 
    [NotEmptyString]string LastName, 
    [NotEmptyString]string Email, 
    [NotEmptyString]string Password, 
    Role Role);