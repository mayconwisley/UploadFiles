using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.UseCases.User.Update;

public sealed record Command(UserUpdateDto UserUpdateDto) : IRequest<Result<Response>>;
