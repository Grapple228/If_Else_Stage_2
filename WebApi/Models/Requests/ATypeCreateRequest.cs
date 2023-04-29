using WebApi.Attributes;

namespace WebApi.Requests;

public record ATypeCreateRequest(
    [NotEmptyString] string Type);