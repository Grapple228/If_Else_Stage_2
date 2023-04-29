namespace WebApi.Helpers;

internal static class UrlHelpers
{
    public static string GetUrl(this HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host}{request.Path}";
    }
}