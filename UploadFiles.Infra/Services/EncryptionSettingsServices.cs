using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public class EncryptionSettingsServices : IEncryptionSettingsServices
{
	public string? Key { get; }
	public EncryptionSettingsServices()
	{
		Key = Environment.GetEnvironmentVariable("UploadFilesKey", EnvironmentVariableTarget.Machine) ??
			throw new InvalidOperationException("Variavel de Ambiente 'UploadFilesKey' não encontrada");
	}
}
