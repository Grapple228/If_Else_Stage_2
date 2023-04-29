using Database;
using Database.Filters;
using Database.Models;
using WebApi.Converters;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Helpers;
using WebApi.Misc;
using WebApi.Models;
using WebApi.Requests;

namespace WebApi.Services;

public class AreasService : ServiceBase, IAreasService
{
    public AreasService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public AreaDto Get(long areaId)
    {
        var area = UnitOfWork.AreasRepository.Get(areaId);
        if (area == null) throw new NotFoundException(ErrorMessages.AreaNotFound);
        return area.AsDto();
    }

    private void CheckCreateRequest(AreaCreateRequest request)
    {
        var points = request.AreaPoints.Select(x => new AreaLocation()
        {
            Longitude = x.Longitude!.Value, Latitude = x.Latitude!.Value
        }).ToArray();
        
        // Количество точек меньше 3
        if (points.Length < 3) throw new BadRequestException(ErrorMessages.AreaHaveLessThanThreePoints);
        
        // Имеются дубликаты точек
        if (points.Distinct().Count() != points.Length)
            throw new BadRequestException(ErrorMessages.AreaHaveDuplicatePoints);

        // Точки находятся на одной прямой
        if (points.IsOnLine()) throw new BadRequestException(ErrorMessages.AreaLocationsOnOneLine);
        
        // Границы пересекаются между собой
        var lines = points.GetSegments();
        if (lines.IsIntersects()) throw new BadRequestException(ErrorMessages.AreaBordersIntersects);

        if (UnitOfWork.AreasRepository.GetWithName(request.Name.ToLower()) != null)
            throw new ConflictException(ErrorMessages.AreaNameExists);
    }

    private void CheckAreaIntersection(Area area)
    {
        // Зона, состоящая из таких точек, уже существует
        var areaPoints = area.AreaLocations.ToArray();
        var ar = UnitOfWork.AreasRepository.GetAll(x => x.Id != area.Id && x.AreaLocations.Count == areaPoints.Length).ToArray();
        if(ar.Any(x => x.AreaLocations.All(t => areaPoints.Any(a => a.Latitude == t.Latitude && a.Longitude == t.Longitude))))
            throw new ConflictException(ErrorMessages.AreaWithLocationsExists);
        
        // Зона пересекается с другими зонами
        foreach (var a in UnitOfWork.AreasRepository.GetAll(x => x.Id != area.Id))
        {
            if (a.IsIntersects(area)) throw new BadRequestException(ErrorMessages.AreaIntersects);
            if (area.IsIntersects(a)) throw new BadRequestException(ErrorMessages.AreaIntersects);
        }

       
    }

    public AreaDto Create(AreaCreateRequest request)
    {
        CheckCreateRequest(request);
        
        var newArea = new Area()
        {
            Name = request.Name.ToLower(),
            AreaLocations = request.AreaPoints
                .Select(x => new AreaLocation() 
                    { Latitude = x.Latitude!.Value, Longitude = x.Longitude!.Value }).ToList()
        };

        CheckAreaIntersection(newArea);

        UnitOfWork.AreasRepository.Create(newArea);
        UnitOfWork.Save();

        return newArea.AsDto();
    }

    public AreaDto Update(long areaId, AreaCreateRequest request)
    {
        var existing = UnitOfWork.AreasRepository.Get(areaId);
        if (existing == null) throw new NotFoundException(ErrorMessages.AreaNotFound);
        
        CheckCreateRequest(request);

        var areaPoints = request.AreaPoints.Select(x => new AreaLocation()
        {
            Latitude = x.Latitude!.Value,
            Longitude = x.Longitude!.Value
        }).ToList();
        
        existing.Name = request.Name.ToLower();
        existing.AreaLocations = areaPoints;
        
        CheckAreaIntersection(existing);

        UnitOfWork.AreasRepository.Update(existing);
        UnitOfWork.Save();
        
        return existing.AsDto();
    }

    public void Delete(long areaId)
    {
        var area = UnitOfWork.AreasRepository.Get(areaId);
        if (area == null) throw new NotFoundException(ErrorMessages.AreaNotFound);
        UnitOfWork.AreasRepository.Delete(area);
        UnitOfWork.Save();
    }

    private IQueryable<Animal> GetAnimalsInArea(long[] areaLocationIds, DateTime startDateTime, DateTime endDateTime)
    {
        var filter = new VisitedLocationFilter()
        {
            StartDateTime = startDateTime,
            EndDateTime = endDateTime
        };
        
        // Получение всех посещенных локаций в границах зоны за период 
        var visitedAreaLocations = UnitOfWork.VisitedLocationsRepository.GetAll(filter)
            .Where(x => areaLocationIds.Contains(x.LocationId)).Select(x => x.Id).ToArray();

        return from animal in UnitOfWork.AnimalsRepository.GetAll(a =>
                // Находится в точке чипирования и не перемещалось
                (areaLocationIds.Contains(a.ChippingLocationId) 
                 && (a.VisitedLocations.Any()
                     // Чипировано в этой зоне и не перемещалось до периода
                     || !a.VisitedLocations
                         .Where(x => x.DateTimeOfVisitLocationPoint <= startDateTime)
                         .OrderBy(o => o.DateTimeOfVisitLocationPoint)
                         .Any()))
                // Переместилось в зону
                || a.VisitedLocations.Any(l => visitedAreaLocations.Contains(l.Id))
                // Последняя точка локации до начала отбора - точка зоны
                || (a.VisitedLocations.Any(l => l.DateTimeOfVisitLocationPoint <= startDateTime)
                    && areaLocationIds.Contains(a.VisitedLocations
                            .Where(w => w.DateTimeOfVisitLocationPoint <= startDateTime)
                            .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                            .Last(l => l.DateTimeOfVisitLocationPoint <= startDateTime).LocationId))) 
            select animal;
    }

    public AnalyticDto GetAnalytic(long areaId, DateTime? startDate, DateTime? endDate)
    {
        if (startDate >= endDate) throw new BadRequestException(ErrorMessages.InvalidDate);

        var startDateTime = startDate?.ToUniversalTime() ?? new DateTime(2000, 1, 1);
        var endDateTime = endDate?.ToUniversalTime() ?? new DateTime(3000, 12, 31);

        var area = UnitOfWork.AreasRepository.Get(areaId);
        if (area == null) throw new NotFoundException(ErrorMessages.AreaNotFound);

        // Получить локации, которые попадают в пределы зоны
        var areaPoints = area.AreaLocations.ToArray();
        var locations = UnitOfWork.LocationsRepository.GetAll().ToArray();
        var areaLocations = locations.Where(x => areaPoints.IsInside(x, false)).ToArray();
        // Идентификаторы точек, которые находятся в зоне
        var areaLocationIds = areaLocations.Select(x => x.Id).ToArray();
        
        // Получение животных
        var animals = GetAnimalsInArea(areaLocationIds, startDateTime, endDateTime)
            .Select(a => new
            {
                AnimalId = a.Id,
                a.ChippingLocationId,
                a.AnimalTypes,
                StartLocationId = a.VisitedLocations.Any(x => x.DateTimeOfVisitLocationPoint <= startDateTime) 
                    ? a.VisitedLocations
                        .Where(w => w.DateTimeOfVisitLocationPoint <= startDateTime)
                        .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                        .Last(l => l.DateTimeOfVisitLocationPoint <= startDateTime).LocationId 
                    : a.ChippingLocationId,
                VisitedLocations = a.VisitedLocations
                    .Where(v => v.DateTimeOfVisitLocationPoint >= startDateTime && v.DateTimeOfVisitLocationPoint <= endDateTime)
                    .OrderBy(x => x.DateTimeOfVisitLocationPoint).ToArray()
            }).OrderBy(x => x.AnimalId).ToArray();
        
        var totalQuantityAnimals = 0L;
        var totalAnimalGone = 0L;
        var totalAnimalArrived = 0L;
        
        var animalAnalytics = new List<AnimalAnalytic>();

        foreach (var animal in animals)
        {
            var currentLocationId = animal.StartLocationId;
            var isInAreaStatus = areaLocationIds.Contains(currentLocationId);
            
            var animalGone = 0;
            var animalArrived = 0;

            var isGoneProcessed = false;
            var isArriveProcessed = false;
            
            // Отслеживание перемещений между зонами
            foreach (var visitedLocation in animal.VisitedLocations)
            {
                currentLocationId = visitedLocation.LocationId;
                var location = areaLocations.SingleOrDefault(x => x.Id == visitedLocation.LocationId);
                if (location != null) // ПОСЕЩЕНИЕ ЛОКАЦИИ ИЗ ЗОНЫ
                {
                    // Если животное в зоне
                    if(isInAreaStatus) continue;
                    
                    isInAreaStatus = true;
                    
                    if (isArriveProcessed) continue;
                    
                    isArriveProcessed = true;
                    animalArrived++;
                    totalAnimalArrived++;
                }
                else // ЖИВОТНОЕ ПОСЕЩАЕТ ЛОКАЦИЮ НЕ ИЗ ЗОНЫ
                {
                    // Если животное не в зоне
                    if(!isInAreaStatus) continue;
                    isInAreaStatus = false;
                    
                    if (isGoneProcessed) continue;
                    
                    isGoneProcessed = true;
                    animalGone++;
                    totalAnimalGone++;
                }
            }

            totalQuantityAnimals += areaLocationIds.Contains(currentLocationId) ? 1 : 0;

            // Обработка типов животного
            foreach (var animalType in animal.AnimalTypes)
            {
                var analytic = animalAnalytics.SingleOrDefault(x => x.AnimalTypeId == animalType.TypeId);
                var quantity = isInAreaStatus ? 1 : 0;
                var arrived = animalArrived == 0 ? 0 : 1;
                var gone = animalGone == 0 ? 0 : 1;
                
                if (analytic == null)
                {
                    var type = UnitOfWork.TypesRepository.Get(animalType.TypeId)!;
                    animalAnalytics.Add(new AnimalAnalytic(type.Type, type.Id)
                    {
                        QuantityAnimals = quantity,
                        AnimalsArrived = arrived,
                        AnimalsGone = gone
                    });
                }
                else
                {
                    analytic.QuantityAnimals += quantity;
                    analytic.AnimalsArrived += arrived;
                    analytic.AnimalsGone += gone;
                }
            }
        }

        return new AnalyticDto(
            totalQuantityAnimals, 
            totalAnimalArrived,
            totalAnimalGone,
            animalAnalytics.AsDto()
        );
    }
}