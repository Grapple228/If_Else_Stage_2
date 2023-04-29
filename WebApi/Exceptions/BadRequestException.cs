using System.Net;

namespace WebApi.Exceptions;

public class BadRequestException : ExceptionBase
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
    {
    }
}