using Microsoft.EntityFrameworkCore;
using UploadFiles.Domain.Entities;
using UploadFiles.Domain.Repositories;
using UploadFiles.Infra.Database;

namespace UploadFiles.Infra.Repositories;

public class UserRepository(UploadFilesDbContext _uploadFilesDbContext) : IUserRepository
{
	public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
	{
		await _uploadFilesDbContext.Users.AddAsync(user, cancellationToken);
		return user;
	}

	public async Task<User> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var user = await GetByIdAsync(id, cancellationToken) ??
			throw new KeyNotFoundException($"Dados com o id {id} não encontrado para exclusão");

		_uploadFilesDbContext.Users.Remove(user);
		return user;
	}

	public async Task<IEnumerable<User>?> GetAllAsync(CancellationToken cancellationToken = default)
		=> await _uploadFilesDbContext.Users
			.OrderBy(o => o.Username)
			.ToListAsync(cancellationToken) ?? null;

	public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await _uploadFilesDbContext.Users
			.FindAsync([id], cancellationToken) ?? null;

	public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
		=> await _uploadFilesDbContext.Users
			.FirstOrDefaultAsync(f => f.Username == username, cancellationToken) ?? null;
}
