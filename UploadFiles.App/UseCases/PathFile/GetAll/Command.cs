using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.GetAll;

public sealed record Command() : IRequest<Result<Response>>;
