using System.ComponentModel.DataAnnotations;
using Database.Enums;
using WebApi.Attributes;

namespace WebApi.Requests;

public record AnimalCreateRequest(
    [MinLength(1)] IEnumerable<long> AnimalTypes, 
    [FloatGreaterThanZero]float Weight,
    [FloatGreaterThanZero]float Length,
    [FloatGreaterThanZero]float Height,
    Gender Gender,
    [Range(1, int.MaxValue)] int ChipperId,
    [Range(1, long.MaxValue)] long ChippingLocationId
);