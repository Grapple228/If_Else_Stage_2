using System.Text.Json;

namespace WebApi.Converters;

internal class UpperCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToUpper();
    }
}