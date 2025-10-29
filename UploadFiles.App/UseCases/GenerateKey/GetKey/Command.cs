using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.GenerateKey.Enum;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.GenerateKey.GetKey;

public sealed record Command(BytesEnum BytesEnum) : IRequest<Result<Response>>;
