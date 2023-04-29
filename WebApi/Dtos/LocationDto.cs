using System.Diagnostics.CodeAnalysis;

namespace WebApi.Dtos;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record LocationDto(
    long Id, 
    double? Latitude, 
    double? Longitude
    );