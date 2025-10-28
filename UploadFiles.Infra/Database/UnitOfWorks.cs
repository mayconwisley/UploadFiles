using UploadFiles.Domain.Abstractions;

namespace UploadFiles.Infra.Database;

public class UnitOfWorks(UploadFilesDbContext _uploadFilesDbContext) : IUnitOfWorks
{
    public async Task CommitAsync() => await _uploadFilesDbContext.SaveChangesAsync();
}
