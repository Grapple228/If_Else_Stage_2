using WebApi.Dtos;
using WebApi.Models.Search;
using WebApi.Requests;

namespace WebApi.Services;

public interface IVisitedLocationService
{
    IEnumerable<VisitedLocationDto> Search(long animalId, VisitedLocationSearch search);
    VisitedLocationDto AddLocationToAnimal(long animalId, long pointId);
    VisitedLocationDto ChangeAnimalLocation(long animalId, VisitedLocationUpdateRequest request);
    void DeleteLocationFromAnimal(long animalId, long visitedPointId);
}