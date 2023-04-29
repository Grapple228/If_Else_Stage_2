using System.Diagnostics.CodeAnalysis;
using Database.Enums;

namespace WebApi.Dtos;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record AnimalDto(
    long Id,
    IEnumerable<long> AnimalTypes,
    float Weight,
    float Length,
    float Height,
    Gender Gender,
    LifeStatus LifeStatus,
    DateTime ChippingDateTime,
    int ChipperId,
    long ChippingLocationId,
    IEnumerable<long> VisitedLocations,
    DateTime? DeathDateTime = null
);