using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.Login;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.Login.Sign;

public sealed record Command(LoginDto LoginDto) : IRequest<Result<Response>>;
