using System.Net;

namespace WebApi.Exceptions;

public class ConflictException : ExceptionBase
{
    public ConflictException(string message) : base(HttpStatusCode.Conflict, message)
    {
    }
}