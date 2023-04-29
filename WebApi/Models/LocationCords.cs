using System.ComponentModel.DataAnnotations;
using WebApi.Misc;

namespace WebApi.Models;

public class LocationCords
{
    [Range(-180.0d, 180.0d, ErrorMessage = ErrorMessages.InvalidLocationCoordinates)] public double Longitude  { get; set; }
    [Range(-90.0d, 90.0d, ErrorMessage = ErrorMessages.InvalidLocationCoordinates)] public double Latitude { get; set; }
}