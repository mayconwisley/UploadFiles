using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.PathFile.Create;

public sealed record Command(PathFileCreateDto PathFileCreateDto) : IRequest<Result<Response>>;
