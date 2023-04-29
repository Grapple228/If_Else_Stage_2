using WebApi.Attributes;

namespace WebApi.Requests;

public record AccountRegisterRequest(
    [NotEmptyString]string FirstName, 
    [NotEmptyString]string LastName, 
    [NotEmptyString]string Email, 
    [NotEmptyString]string Password);