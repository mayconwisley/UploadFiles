using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.GetById;

public sealed record Command(Guid Id) : IRequest<Result<Response>>;
