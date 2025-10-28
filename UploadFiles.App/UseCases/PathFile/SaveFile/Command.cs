using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.SaveFile;

public sealed record Command(Stream FileStream, string Name) : IRequest<Result<Response>>;
