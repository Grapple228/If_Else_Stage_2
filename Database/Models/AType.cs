namespace Database.Models;

public class AType : IModel<long>
{
    public long Id { get; init; }

    public string Type { get; set; } = null!;
}