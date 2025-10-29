using UploadFiles.App.Dtos.User;

namespace UploadFiles.App.UseCases.User.GetAll;

public sealed record Response(IEnumerable<UserOutputDto> UserOutputDtos);
