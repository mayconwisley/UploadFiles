using System.Security.Cryptography;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public sealed class GenerateKeyService : IGenerateKeyService
{
	public Task<string> GetGenerateKey()
	{
		var bytes = new byte[32];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(bytes);
		string key = Convert.ToBase64String(bytes);
		return Task.FromResult(key);
	}
}
