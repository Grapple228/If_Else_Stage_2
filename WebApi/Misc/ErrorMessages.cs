namespace WebApi.Misc;

internal static class ErrorMessages
{
    public const string AccountLinkedToAnimal = "Аккаунт связан с животным";
    public const string AccountNotFound = "Аккаунт не найден";
    public const string AnimalHaveLocations = "Животное покинуло точку локации, при этом имеются другие посещенные точки";
    public const string AnimalHaveNoLocation = "У животного отсутствует точка";
    public const string AnimalHaveNoType = "У животного нет типа";
    public const string AnimalHaveOneType = "У животного один тип";
    public const string AnimalHaveType = "У животного есть такой тип";
    public const string AnimalNotFound = "Животное не найдено";
    public const string AreaBordersIntersects = "Границы зоны пересекаются";
    public const string AreaHaveDuplicatePoints = "Имеются одинаковые точки";
    public const string AreaHaveLessThanThreePoints = "Передано меньше трех точек";
    public const string AreaIntersects = "Зона пересекается с другими зонами";
    public const string AreaLocationsOnOneLine = "Точки локации расположены на одной линии";
    public const string AreaNameExists = "Зона с таким именем существует";
    public const string AreaNotFound = "Зона не найдена";
    public const string AreaWithLocationsExists = "Зона с такими вершинами существует";
    public const string DeletingChippingLocation = "Удаление локации чипирования, связанной с животным";
    public const string EmailExists = "Почта уже зарегистрирована";
    public const string EmptyTypes = "Типы не указаны";
    public const string EqualLocations = "Одинаковые точки";
    public const string EqualToNextOrPrevLocation = "Точка совпадает со следующей или предыдущей точкой";
    public const string ExistAnimalWithType = "Есть животное с таким типом";
    public const string Forbidden = "Недостаточно прав!";
    public const string InvalidData = "Некорректные данные";
    public const string InvalidDate = "Некорректная дата";
    public const string InvalidLifeStatus = "Попытка установить данные для животного со статусом DEAD";
    public const string InvalidLocationCoordinates = "Неверные координаты";
    public const string InvalidNumberParameters = "Невалидные числовые параметры";
    public const string LastLocationEqualToChipping = "Точка чипирования совпадает с первой посещенной точкой локации";
    public const string LocationAlreadyExists = "Точка локации уже существует";
    public const string LocationEqualsToChippingLocation = "Попытка добавить точку, равную точке чипирования";
    public const string LocationEqualsToCurrentLocation = "Попытка добавить точку, равную текущей точке";
    public const string LocationLinkedToAnimal = "Точка локации связана с животным";
    public const string LocationNotFound = "Точка не найдена";
    public const string TryingToChangeRole = "Попытка изменить роль";
    public const string TypeAlreadyExists = "Тип уже существует";
    public const string TypeNotFound = "Тип животного не найден";
}