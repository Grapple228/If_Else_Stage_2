using System.ComponentModel.DataAnnotations;

namespace WebApi.Attributes;

public class NotEmptyStringAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var str = value?.ToString();
        return !string.IsNullOrWhiteSpace(str);
    }
}