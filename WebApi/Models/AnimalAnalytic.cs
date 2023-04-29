namespace WebApi.Models;

public class AnimalAnalytic
{
    public AnimalAnalytic(string animalType, long animalTypeId)
    {
        AnimalType = animalType;
        AnimalTypeId = animalTypeId;
    }
    
    public string AnimalType { get; }
    public long AnimalTypeId { get; }
    public long QuantityAnimals { get; set; }
    public long AnimalsArrived { get; set; }
    public long AnimalsGone { get; set; }
}