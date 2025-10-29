using UploadFiles.Domain.Entities;

namespace UploadFiles.Domain.Repositories;

public interface IUserRepository
{
	Task<IEnumerable<User>?> GetAllAsync(CancellationToken cancellationToken = default);
	Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
	Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
	Task<User> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
