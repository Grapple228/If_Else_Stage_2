namespace WebApi.Dtos;

public record AreaDto(long Id, string Name, IEnumerable<AreaLocationDto> AreaPoints);