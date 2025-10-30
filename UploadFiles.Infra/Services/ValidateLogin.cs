using UploadFiles.Domain.Repositories;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public sealed class ValidateLogin(IUserRepository _userRepository, IEncryptionService _encryptionService,
	IEncryptionSettingsService _encryptionSettingsService) : IValidateLogin
{
	public async Task<bool> IsValidateLogin(string username, string password)
	{
		var key = _encryptionSettingsService.Key;
		if (key is null)
			return false;

		var entity = await _userRepository.GetByUsernameAsync(username);
		if (entity is null)
			return false;

		var passwordDecryption = _encryptionService.Decrypt(_encryptionService.Text(entity.Password), key, _encryptionService.IV(entity.Password));
		if (!password.Equals(passwordDecryption))
			return false;

		return true;
	}
}
