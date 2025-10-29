using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.User.GetAll;

public sealed record Command() : IRequest<Result<Response>>;
