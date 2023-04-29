using System.Diagnostics.CodeAnalysis;
using Database.Enums;

namespace WebApi.Dtos;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record AccountDto(
    int Id, 
    string FirstName, 
    string LastName, 
    string Email,
    Role Role
    );