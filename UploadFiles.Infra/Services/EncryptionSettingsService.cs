using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public class EncryptionSettingsService : IEncryptionSettingsService
{
	public string? Key { get; }
	public EncryptionSettingsService()
	{
		Key = Environment.GetEnvironmentVariable("UploadFilesKey", EnvironmentVariableTarget.Machine) ??
			throw new InvalidOperationException("Variavel de Ambiente 'UploadFilesKey' não encontrada");
	}
}
