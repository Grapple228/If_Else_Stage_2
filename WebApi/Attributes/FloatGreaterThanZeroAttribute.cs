using System.ComponentModel.DataAnnotations;

namespace WebApi.Attributes;

public class FloatGreaterThanZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value != null && float.TryParse(value.ToString(), out var i) && i > 0;
    }
}