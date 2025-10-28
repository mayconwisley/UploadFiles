using System.Net;

namespace UploadFiles.Domain.Abstractions;

public record Error(HttpStatusCode StatusCode, string Message)
{
    public static Error None(string message) => new(HttpStatusCode.OK, message);
    public static Error NullValue(string message) => new(HttpStatusCode.BadRequest, message);
    public static Error NotFound(string message) => new(HttpStatusCode.NotFound, message);
    public static Error BadRequest(string message) => new(HttpStatusCode.BadRequest, message);
    public static Error InternalServer(string message) => new(HttpStatusCode.InternalServerError, message);
    public static Error Unauthorized(string message) => new(HttpStatusCode.Unauthorized, message);
    public static Error Forbidden(string message) => new(HttpStatusCode.Forbidden, message);
    public static Error Conflict(string message) => new(HttpStatusCode.Conflict, message);
    public static Error Validation(string message) => new(HttpStatusCode.UnprocessableEntity, message);
    public static Error NotImplemented(string message) => new(HttpStatusCode.NotImplemented, message);
    public static Error ServiceUnavailable(string message) => new(HttpStatusCode.ServiceUnavailable, message);
    public static Error MethodNotAllowed(string message) => new(HttpStatusCode.MethodNotAllowed, message);
}
