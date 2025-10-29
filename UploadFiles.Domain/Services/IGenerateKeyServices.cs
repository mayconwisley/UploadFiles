namespace UploadFiles.Domain.Services;

public interface IGenerateKeyServices
{
	Task<string> GetGenerateKey(byte[] bytes);
}
