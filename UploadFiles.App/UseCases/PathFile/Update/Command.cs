using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.Update;

public sealed record Command(PathFileUpdateDto PathFileUpdateDto) : IRequest<Result<Response>>;
