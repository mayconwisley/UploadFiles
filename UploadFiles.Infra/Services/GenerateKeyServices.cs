using System.Security.Cryptography;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public sealed class GenerateKeyServices : IGenerateKeyServices
{
	public Task<string> GetGenerateKey(byte[] bytes)
	{
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(bytes);
		string key = Convert.ToBase64String(bytes);
		return Task.FromResult(key);
	}
}
