using System.Net;

namespace WebApi.Exceptions;

public class ForbiddenException : ExceptionBase
{
    public ForbiddenException(string message) : base(HttpStatusCode.Forbidden, message)
    {
    }
}