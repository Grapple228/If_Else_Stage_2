using WebApi.Models;

namespace WebApi.Dtos;

public record AnalyticDto(
    long TotalQuantityAnimals,
    long TotalAnimalsArrived,
    long TotalAnimalsGone,
    IEnumerable<AnimalAnalyticDto> AnimalsAnalytics);
public record AnimalAnalyticDto(
    string AnimalType,
    long AnimalTypeId,
    long QuantityAnimals,
    long AnimalsArrived,
    long AnimalsGone
);

public static class Extensions
{
    private static AnimalAnalyticDto AsDto(this AnimalAnalytic analytic) =>
        new(analytic.AnimalType, analytic.AnimalTypeId, analytic.QuantityAnimals,
            analytic.AnimalsArrived, analytic.AnimalsGone);

    public static IEnumerable<AnimalAnalyticDto> AsDto(this IEnumerable<AnimalAnalytic> analytics) =>
        analytics.Select(x => x.AsDto());
}