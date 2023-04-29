using WebApi.Attributes;

namespace WebApi.Requests;

public record AreaCreateRequest(
    [NotEmptyString]string Name,
        IEnumerable<LocationCreateRequest> AreaPoints);