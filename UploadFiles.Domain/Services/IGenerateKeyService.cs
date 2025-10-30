namespace UploadFiles.Domain.Services;

public interface IGenerateKeyService
{
	Task<string> GetGenerateKey();
}
