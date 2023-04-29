namespace Database.Models;

/// <summary>
/// Интерфейс, позволяющий использовать модель в generic репозитории RepositoryBase
/// </summary>
/// <typeparam name="TId">Тип данных для поля Id</typeparam>
public interface IModel<TId>  where TId : struct 
{
    TId Id { get; init; }
}