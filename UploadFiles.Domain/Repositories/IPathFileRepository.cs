using UploadFiles.Domain.Entities;

namespace UploadFiles.Domain.Repositories;

public interface IPathFileRepository
{
	Task<PathFile?> GetAsync(CancellationToken cancellationToken = default);
	Task<PathFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<PathFile> CreateAsync(PathFile pathFile, CancellationToken cancellationToken = default);
	Task<PathFile> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
