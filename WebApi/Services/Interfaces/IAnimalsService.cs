using WebApi.Dtos;
using WebApi.Models.Search;
using WebApi.Requests;

namespace WebApi.Services;

public interface IAnimalsService
{
    AnimalDto Get(long animalId);
    IEnumerable<AnimalDto> Search(AnimalSearch search);
    AnimalDto Create(AnimalCreateRequest request);
    AnimalDto Update(long animalId, AnimalUpdateRequest request);
    void Delete(long animalId);
    AnimalDto AddTypeToAnimal(long animalId, long typeId);
    AnimalDto ReplaceTypeToAnimal(long animalId, ATypeReplaceRequest replaceRequest);
    AnimalDto DeleteTypeFromAnimal(long animalId, long typeId);
}