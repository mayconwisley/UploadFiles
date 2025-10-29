namespace UploadFiles.Infra.Services;

public class EncryptionSettingsServices
{
	public string? Key { get; private set; }
	public EncryptionSettingsServices()
	{
		Key = Environment.GetEnvironmentVariable("UploadFilesKey", EnvironmentVariableTarget.Machine) ??
			throw new InvalidOperationException("Variavel de Ambiente 'UploadFilesKey' não encontrada");
	}
}
