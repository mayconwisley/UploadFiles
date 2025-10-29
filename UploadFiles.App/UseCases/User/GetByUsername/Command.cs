using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.User.GetByUsername;

public sealed record Command(string Username) : IRequest<Result<Response>>;
