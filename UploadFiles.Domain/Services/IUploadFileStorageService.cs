namespace UploadFiles.Domain.Services;

public interface IUploadFileStorageService
{
    Task<bool> SaveFileAsync(Stream fileStream, string name);
}
