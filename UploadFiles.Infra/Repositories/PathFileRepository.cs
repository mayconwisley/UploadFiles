using Microsoft.EntityFrameworkCore;
using UploadFiles.Domain.Entities;
using UploadFiles.Domain.Repositories;
using UploadFiles.Infra.Database;

namespace UploadFiles.Infra.Repositories;

public class PathFileRepository(UploadFilesDbContext _uploadFilesContext) : IPathFileRepository
{
    public async Task<PathFile> CreateAsync(PathFile pathFile, CancellationToken cancellationToken = default)
    {
        await _uploadFilesContext.PathFiles.AddAsync(pathFile, cancellationToken);
        return pathFile;
    }

    public async Task<PathFile> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pathFile = await GetByIdAsync(id, cancellationToken) ??
            throw new KeyNotFoundException($"Dados com o id {id} não encontrado para exclusão");

        _uploadFilesContext.PathFiles.Remove(pathFile);
        return pathFile;
    }

    public async Task<PathFile?> GetAsync(CancellationToken cancellationToken = default)
        => await _uploadFilesContext.PathFiles
            .OrderBy(o => o.Path)
            .FirstOrDefaultAsync(cancellationToken) ?? null;

    public async Task<PathFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _uploadFilesContext.PathFiles
            .FindAsync([id], cancellationToken) ?? null;

    public async Task<PathFile> UpdateAsync(PathFile pathFile, CancellationToken cancellationToken = default)
    {
        if (pathFile is null)
            throw new ArgumentNullException(nameof(pathFile), "Local do Arquivo não pode ser nulo.");

        if (pathFile.Id == Guid.Empty)
            throw new ArgumentException("Local do Arquivo deve ter um Id válido.", nameof(pathFile));

        var connetioConfigExists = await GetByIdAsync(pathFile.Id, cancellationToken) ??
            throw new KeyNotFoundException($"Local do Arquivo com id {pathFile.Id} não encontrado.");

        _uploadFilesContext.Entry(connetioConfigExists).CurrentValues.SetValues(pathFile);

        return pathFile;
    }
}
