using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.Get;

public sealed record Command() : IRequest<Result<Response>>;
