using Database.Models;
using WebApi.Dtos;

namespace WebApi.Converters;

public static class DtoConverter
{
    internal static AccountDto AsDto(this Account model)
    {
        return new AccountDto(model.Id, model.FirstName, model.LastName, model.Email, model.Role);
    }

    internal static IEnumerable<AccountDto> AsDto(this IEnumerable<Account> models)
    {
        return models.Select(AsDto);
    }
    
    internal static AnimalDto AsDto(this Animal model)
    {
        return new AnimalDto(model.Id, model.AnimalTypes.Select(x => x.TypeId),
            model.Weight, model.Length, model.Height, model.Gender, model.LifeStatus,
            model.ChippingDateTime, model.ChipperId, model.ChippingLocationId,
            model.VisitedLocations.Select(x => x.Id), model.DeathDateTime);
    }

    internal static IEnumerable<AnimalDto> AsDto(this IEnumerable<Animal> models)
    {
        return models.Select(AsDto);
    }
    
    internal static AreaDto AsDto(this Area model)
    {
        return new AreaDto(model.Id, model.Name,
            model.AreaLocations.Select(x => new AreaLocationDto(x.Longitude, x.Latitude)));
    }

    internal static IEnumerable<AreaDto> AsDto(this IEnumerable<Area> models)
    {
        return models.Select(AsDto);
    }
    
    internal static LocationDto AsDto(this Location model)
    {
        return new LocationDto(model.Id, model.Latitude, model.Longitude);
    }

    internal static IEnumerable<LocationDto> AsDto(this IEnumerable<Location> models)
    {
        return models.Select(AsDto);
    }
    
    internal static VisitedLocationDto AsDto(this VisitedLocation model)
    {
        return new VisitedLocationDto(model.Id, model.DateTimeOfVisitLocationPoint, model.LocationId);
    }

    internal static IEnumerable<VisitedLocationDto> AsDto(this IEnumerable<VisitedLocation> models)
    {
        return models.Select(AsDto);
    }
    
    internal static ATypeDto AsDto(this AType model)
    {
        return new ATypeDto(model.Id, model.Type);
    }

    internal static IEnumerable<ATypeDto> AsDto(this IEnumerable<AType> models)
    {
        return models.Select(AsDto);
    }
}