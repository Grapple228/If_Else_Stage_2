using System.ComponentModel.DataAnnotations;
using WebApi.Misc;

namespace WebApi.Requests;

public record LocationCreateRequest(
    [Required][Range(-180.0d, 180.0d, ErrorMessage = ErrorMessages.InvalidLocationCoordinates)] double? Longitude = null, 
    [Required][Range(-90.0d, 90.0d, ErrorMessage = ErrorMessages.InvalidLocationCoordinates)] double? Latitude = null);