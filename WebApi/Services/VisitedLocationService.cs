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

public class VisitedLocationService : ServiceBase, IVisitedLocationService
{
    public VisitedLocationService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public IEnumerable<VisitedLocationDto> Search(long animalId, VisitedLocationSearch search)
    {
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);

        var filter = new VisitedLocationFilter()
        {
            AnimalId = animalId,
            StartDateTime = search.StartDateTime,
            EndDateTime = search.EndDateTime
        };
        return UnitOfWork.VisitedLocationsRepository.GetAll(filter)
            .OrderBy(x => x.DateTimeOfVisitLocationPoint).Skip(search.From).Take(search.Size).AsEnumerable().AsDto();
    }

    public VisitedLocationDto AddLocationToAnimal(long animalId, long pointId)
    {
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);

        if (animal.LifeStatus == LifeStatus.Dead)
            throw new BadRequestException(ErrorMessages.InvalidLifeStatus);

        var pointToAdd = UnitOfWork.LocationsRepository.Get(pointId);
        if (pointToAdd == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        if (animal.VisitedLocations.Any())
        {
            if (animal.VisitedLocations.Last().LocationId == pointId)
                throw new BadRequestException(ErrorMessages.LocationEqualsToCurrentLocation);
        }
        else
        {
            if (pointToAdd.Id == animal.ChippingLocationId)
                throw new BadRequestException(ErrorMessages.LocationEqualsToChippingLocation);
        }

        var point = new VisitedLocation
        {
            LocationId = pointId
        };
        
        animal.VisitedLocations.Add(point);
        
        UnitOfWork.AnimalsRepository.Update(animal);
        UnitOfWork.Save();

        return point.AsDto();
    }

    public VisitedLocationDto ChangeAnimalLocation(long animalId, VisitedLocationUpdateRequest request)
    {
        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);
        
        // Точка, которую нужно изменить
        var visitedPoint = UnitOfWork.VisitedLocationsRepository.Get(request.VisitedLocationPointId);
        if (visitedPoint == null) throw new NotFoundException(ErrorMessages.LocationNotFound);
        
        // Точка, на которую произойдет замена в посещенной точке
        var pointToAdd = UnitOfWork.LocationsRepository.Get(request.LocationPointId);
        if (pointToAdd == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        if (visitedPoint.LocationId == pointToAdd.Id) throw new BadRequestException(ErrorMessages.EqualLocations);

        var animalPoint = animal.VisitedLocations.SingleOrDefault(x => x.Id == visitedPoint.Id);
        if (animalPoint == null) throw new NotFoundException(ErrorMessages.AnimalHaveNoLocation);

        var animalLocations = animal.VisitedLocations.ToList();
        var pointIndex = animalLocations.IndexOf(animalPoint);
        if (animal.ChippingLocationId == pointToAdd.Id && pointIndex == 0)
            throw new BadRequestException(ErrorMessages.LocationEqualsToChippingLocation);

        var visitedPointsCount = animal.VisitedLocations.Count;
        if (visitedPointsCount != 1)
        {
            if (pointIndex > 0 && animalLocations[pointIndex - 1].LocationId == pointToAdd.Id)
                throw new BadRequestException(ErrorMessages.EqualToNextOrPrevLocation);
            if (pointIndex < visitedPointsCount - 1 && animalLocations[pointIndex + 1].LocationId == pointToAdd.Id)
                throw new BadRequestException(ErrorMessages.EqualToNextOrPrevLocation);
        }

        animalPoint.LocationId = pointToAdd.Id;
        UnitOfWork.VisitedLocationsRepository.Update(animalPoint);
        UnitOfWork.Save();
        
        return animalPoint.AsDto();
    }

    public void DeleteLocationFromAnimal(long animalId, long visitedPointId)
    {
        var visitedPoint = UnitOfWork.VisitedLocationsRepository.Get(visitedPointId);
        if (visitedPoint == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        var animal = UnitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException(ErrorMessages.AnimalNotFound);
        
        var locationToRemove = animal.VisitedLocations.FirstOrDefault(x => x.Id == visitedPointId);
        if (locationToRemove == null) throw new NotFoundException(ErrorMessages.AnimalHaveNoLocation);

        var visitedLocations = animal.VisitedLocations.ToList();
        var indexOfLocation = visitedLocations.IndexOf(locationToRemove);

        if (visitedLocations.Count >= 2 && indexOfLocation == 0 &&
            visitedLocations[1].LocationId == animal.ChippingLocationId)
        {
            // Удалить 2 точки
            UnitOfWork.VisitedLocationsRepository.Delete(visitedPoint);
            UnitOfWork.VisitedLocationsRepository.Delete(visitedLocations[1].Id);
        }
        else
        {
            // Удалить 1 точку
            UnitOfWork.VisitedLocationsRepository.Delete(visitedPoint);
        }

        UnitOfWork.Save();
    }
}