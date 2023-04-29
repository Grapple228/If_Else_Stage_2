using System.Net;

namespace WebApi.Exceptions;

public abstract class ExceptionBase : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    protected ExceptionBase(HttpStatusCode code, string message) : base(message)
    {
        StatusCode = code;
    }
}