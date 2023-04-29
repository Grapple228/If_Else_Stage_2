using System.Net;

namespace WebApi.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
    {
    }
}