using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests;

public record VisitedLocationUpdateRequest(
    [Required][Range(1, long.MaxValue)] long VisitedLocationPointId,
    [Required][Range(1, long.MaxValue)] long LocationPointId
);