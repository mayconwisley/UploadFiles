using UploadFiles.Domain.Repositories;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public sealed class UploadFileStorageService(IPathFileRepository _pathFileRepository) : IUploadFileStorageService
{
    public async Task<bool> SaveFileAsync(Stream fileStream, string name)
    {
        var fileName = $"{name}";
        var filePath = await _pathFileRepository.GetAsync();

        if (filePath is null)
            return false;

        var path = filePath.Path;
        path = Path.Combine(path, fileName);

        using var file = new FileStream(path, FileMode.Create);
        await fileStream.CopyToAsync(file);
        await fileStream.DisposeAsync();

        return true;
    }
}
