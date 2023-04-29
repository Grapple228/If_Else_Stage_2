using System.Net;

namespace WebApi.Exceptions;

public class UnauthorizedException : ExceptionBase
{
    public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
    {
    }
}