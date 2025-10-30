using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.Login.CreateUserStandard;

public sealed record Command() : IRequest<Result<Response>>;
