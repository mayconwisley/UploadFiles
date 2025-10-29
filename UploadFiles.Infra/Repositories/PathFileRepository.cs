using Microsoft.EntityFrameworkCore;
using UploadFiles.Domain.Entities;
using UploadFiles.Domain.Repositories;
using UploadFiles.Infra.Database;

namespace UploadFiles.Infra.Repositories;

public class PathFileRepository(UploadFilesDbContext _uploadFilesDbContext) : IPathFileRepository
{
	public async Task<PathFile> CreateAsync(PathFile pathFile, CancellationToken cancellationToken = default)
	{
		await _uploadFilesDbContext.PathFiles.AddAsync(pathFile, cancellationToken);
		return pathFile;
	}

	public async Task<PathFile> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var pathFile = await GetByIdAsync(id, cancellationToken) ??
			throw new KeyNotFoundException($"Dados com o id {id} não encontrado para exclusão");

		_uploadFilesDbContext.PathFiles.Remove(pathFile);
		return pathFile;
	}

	public async Task<PathFile?> GetAsync(CancellationToken cancellationToken = default)
		=> await _uploadFilesDbContext.PathFiles
			.OrderBy(o => o.Path)
			.FirstOrDefaultAsync(cancellationToken) ?? null;

	public async Task<PathFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await _uploadFilesDbContext.PathFiles
			.FindAsync([id], cancellationToken) ?? null;
}
