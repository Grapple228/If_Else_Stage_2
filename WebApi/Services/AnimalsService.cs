using Database;
using Database.Enums;
using Database.Filters;
using Database.Models;
using WebApi.Converters;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Misc;
using WebApi.Models.Search;
using WebApi.Requests;

namespace WebApi.Services;

public class AnimalsService : ServiceBase, IAnimalsService
{

    public AnimalsService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public AnimalDto Get(long animalId)
    {
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);

        if (animal == null)
            throw new NotFoundException(ErrorMessages.AnimalNotFound);
        
        return animal.AsDto();
    }
    
    public IEnumerable<AnimalDto> Search(AnimalSearch search)
    {
        var filter = new AnimalFilter
        {
            Gender = search.Gender,
            ChipperId = search.ChipperId,
            LifeStatus = search.LifeStatus,
            ChippingLocationId = search.ChippingLocationId,
            EndDateTime = search.EndDateTime,
            StartDateTime = search.StartDateTime
        };
        return UnitOfWork.AnimalsRepository.GetAll(filter)
            .OrderBy(x => x.Id).Skip(search.From).Take(search.Size).AsEnumerable().AsDto();
    }

    public AnimalDto Create(AnimalCreateRequest request)
    {
        if (!request.AnimalTypes.Any())
            throw new BadRequestException(ErrorMessages.EmptyTypes);

        if (request.AnimalTypes.Any(x => x <= 0))
            throw new BadRequestException(ErrorMessages.InvalidNumberParameters);

        var chipper = UnitOfWork.AccountsRepository.Get(request.ChipperId);
        if (chipper == null) throw new NotFoundException(ErrorMessages.AccountNotFound);
        var chippingLocation = UnitOfWork.LocationsRepository.Get(request.ChippingLocationId);
        if (chippingLocation == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        var types = UnitOfWork.TypesRepository.GetAll(x => request.AnimalTypes.Contains(x.Id)).ToArray();
        
        if (types.Length != request.AnimalTypes.Count())
            throw new NotFoundException(ErrorMessages.TypeNotFound);

        var animalTypes = types.Select(x => new AnimalType
        {
            TypeId = x.Id
        }).ToList();

        var created = new Animal
        {
            Gender = request.Gender,
            Height = request.Height,
            Length = request.Length,
            Weight = request.Weight,
            ChipperId = chipper.Id,
            ChippingLocationId = chippingLocation.Id,
            AnimalTypes = animalTypes
        };
        
        UnitOfWork.AnimalsRepository.Create(created);

        UnitOfWork.Save();

        return created.AsDto();
    }

    public AnimalDto Update(long animalId, AnimalUpdateRequest request)
    {
        var current = UnitOfWork.AnimalsRepository.Get(animalId);
        if (current == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);

        if (current.LifeStatus == LifeStatus.Dead && request.LifeStatus == LifeStatus.Alive)
            throw new BadRequestException(ErrorMessages.InvalidLifeStatus);

        var chippingLocation = UnitOfWork.LocationsRepository.Get(request.ChippingLocationId);
        if (chippingLocation == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        if (current.VisitedLocations.FirstOrDefault()?.LocationId == request.ChippingLocationId)
            throw new BadRequestException(ErrorMessages.LastLocationEqualToChipping);

        var chipper = UnitOfWork.AccountsRepository.Get(request.ChipperId);
        if (chipper == null) throw new NotFoundException(ErrorMessages.AccountNotFound);
        
        current.Gender = request.Gender;
        current.LifeStatus = request.LifeStatus;
        current.Height = request.Height;
        current.Length = request.Length;
        current.Weight = request.Weight;
        current.ChipperId = chipper.Id;
        current.ChippingLocationId = chippingLocation.Id;

        UnitOfWork.AnimalsRepository.Update(current);

        UnitOfWork.Save();
        
        return current.AsDto();
    }

    public void Delete(long animalId)
    {
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null)
            throw new NotFoundException(ErrorMessages.AnimalNotFound);

        if (animal.VisitedLocations.Any())
            throw new BadRequestException(ErrorMessages.AnimalHaveLocations);

        UnitOfWork.AnimalsRepository.Delete(animal);

        UnitOfWork.Save();
    }

    public AnimalDto AddTypeToAnimal(long animalId, long typeId)
    {
        var type = UnitOfWork.TypesRepository.Get(typeId);
        if (type == null) throw new NotFoundException(ErrorMessages.TypeNotFound);
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);

        if (animal.AnimalTypes.SingleOrDefault(x => x.TypeId == typeId) != null)
            throw new ConflictException(ErrorMessages.AnimalHaveType);

        animal.AnimalTypes.Add(new AnimalType
        {
            AnimalId = animalId,
            TypeId = typeId
        });
        
        UnitOfWork.AnimalsRepository.Update(animal);
        
        UnitOfWork.Save();

        return animal.AsDto();
    }

    public AnimalDto ReplaceTypeToAnimal(long animalId, ATypeReplaceRequest replaceRequest)
    {
        if (UnitOfWork.TypesRepository.Get(replaceRequest.OldTypeId) == null)
            throw new NotFoundException(ErrorMessages.TypeNotFound);

        var newType = UnitOfWork.TypesRepository.Get(replaceRequest.NewTypeId);
        if (newType == null) throw new NotFoundException(ErrorMessages.TypeNotFound);
        
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);
        
        var typeToUpdate = animal.AnimalTypes.SingleOrDefault(x => x.TypeId == replaceRequest.OldTypeId);
        var typeToAdd = animal.AnimalTypes.SingleOrDefault(x => x.TypeId == replaceRequest.NewTypeId);

        if (typeToAdd != null && typeToUpdate != null)
            throw new ConflictException(ErrorMessages.AnimalHaveType);
        if (typeToUpdate == null)
            throw new NotFoundException(ErrorMessages.AnimalHaveNoType);
        if (typeToAdd != null)
            throw new ConflictException(ErrorMessages.AnimalHaveType);

        animal.AnimalTypes.Remove(typeToUpdate);
        animal.AnimalTypes.Add(new AnimalType()
        {
            TypeId = newType.Id
        });
        
        UnitOfWork.AnimalsRepository.Update(animal);
        UnitOfWork.Save();

        return animal.AsDto();
    }

    public AnimalDto DeleteTypeFromAnimal(long animalId, long typeId)
    {
        var type = UnitOfWork.TypesRepository.Get(typeId);
        if (type == null) throw new NotFoundException(ErrorMessages.TypeNotFound);
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);
        var typeToRemove = animal.AnimalTypes.SingleOrDefault(x => x.TypeId == typeId);
        if (typeToRemove == null) throw new NotFoundException(ErrorMessages.AnimalHaveNoType);
        if (animal.AnimalTypes.Count == 1) throw new BadRequestException(ErrorMessages.AnimalHaveOneType);

        animal.AnimalTypes.Remove(typeToRemove);
        UnitOfWork.AnimalsRepository.Update(animal);
        UnitOfWork.Save();

        return animal.AsDto();
    }
}