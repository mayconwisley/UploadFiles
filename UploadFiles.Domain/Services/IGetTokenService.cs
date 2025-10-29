using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Entities;

namespace UploadFiles.Domain.Services;

public interface IGetTokenService
{
    public Task<Result<string>> Token(User user);
}
