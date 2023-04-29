using System.ComponentModel.DataAnnotations;
using WebApi.Misc;

namespace WebApi.Requests;

public record ATypeReplaceRequest(
    [Required][Range(1, long.MaxValue, ErrorMessage = ErrorMessages.InvalidNumberParameters)] long OldTypeId, 
    [Required][Range(1, long.MaxValue, ErrorMessage = ErrorMessages.InvalidNumberParameters)] long NewTypeId);