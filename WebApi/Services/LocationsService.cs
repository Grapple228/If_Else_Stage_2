using Database;
using Database.Models;
using WebApi.Converters;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Helpers;
using WebApi.Misc;
using WebApi.Models;
using WebApi.Requests;

namespace WebApi.Services;

public class LocationsService : ServiceBase, ILocationsService
{
    public LocationsService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public LocationDto Get(double latitude, double longitude)
    {
        var location = UnitOfWork.LocationsRepository.GetLocationWithCords(latitude, longitude);
        if (location == null) throw new NotFoundException(ErrorMessages.LocationNotFound);
        return location.AsDto();
    } 
    
    public LocationDto Get(long pointId)
    {
        var location = UnitOfWork.LocationsRepository.Get(pointId);
        
        if (location == null) throw new NotFoundException(ErrorMessages.LocationNotFound);
        
        return location.AsDto();
    }

    public LocationDto CreateLocation(LocationCreateRequest request)
    {
        if (UnitOfWork.LocationsRepository.GetLocationWithCords(request.Latitude!.Value, request.Longitude!.Value) !=
            null)
            throw new ConflictException(ErrorMessages.LocationAlreadyExists);

        var location = new Location
        {
            Latitude = request.Latitude!.Value,
            Longitude = request.Longitude!.Value
        };
        
        UnitOfWork.LocationsRepository.Create(location);
        UnitOfWork.Save();

        return location.AsDto();
    }

    public LocationDto UpdateLocation(long pointId, LocationCreateRequest request)
    {
        var location = UnitOfWork.LocationsRepository.Get(pointId);
        if (location == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        if (UnitOfWork.LocationsRepository
                .GetLocationWithCords(request.Latitude!.Value, request.Longitude!.Value) != null)
            throw new ConflictException(ErrorMessages.LocationAlreadyExists);

        location.Latitude = request.Latitude.Value;
        location.Longitude = request.Longitude.Value;
        
        UnitOfWork.LocationsRepository.Update(location);
        UnitOfWork.Save();

        return location.AsDto();
    }

    public void DeleteLocation(long pointId)
    {
        var location = UnitOfWork.LocationsRepository.Get(pointId);
        if (location == null) throw new NotFoundException(ErrorMessages.LocationNotFound);

        if (UnitOfWork.AnimalsRepository.GetFirstWithLocation(pointId) != null)
            throw new BadRequestException(ErrorMessages.LocationLinkedToAnimal);

        if (UnitOfWork.AnimalsRepository.GetFirstWithChippingLocation(pointId) != null)
            throw new BadRequestException(ErrorMessages.DeletingChippingLocation);

        UnitOfWork.LocationsRepository.Delete(location);
        UnitOfWork.Save();
    }

    public string GetHashV1(LocationCords cords) =>
        cords.GetGeoHash();
    
    public string GetHashV2(LocationCords cords) =>
        GetHashV1(cords)
            .ToBase64String();
    
    public string GetHashV3(LocationCords cords) =>
        GetHashV1(cords)
            .ToBytes()
            .GetReversedMd5Hash()
            .ToBase64String();
}