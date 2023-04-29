using System.Diagnostics.CodeAnalysis;

namespace WebApi.Dtos;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record VisitedLocationDto(
    long Id,
    DateTime DateTimeOfVisitLocationPoint,
    long LocationPointId
);