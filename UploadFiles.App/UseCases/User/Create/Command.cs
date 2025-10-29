using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.User.Create;

public sealed record Command(UserCreateDto UserCreateDto) : IRequest<Result<Response>>;
