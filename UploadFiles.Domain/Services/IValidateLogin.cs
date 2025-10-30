namespace UploadFiles.Domain.Services;

public interface IValidateLogin
{
	public Task<bool> IsValidateLogin(string username, string password);
}
