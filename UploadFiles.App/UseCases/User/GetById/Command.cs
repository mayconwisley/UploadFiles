using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.User.GetById;

public sealed record Command(Guid Id) : IRequest<Result<Response>>;
