namespace UploadFiles.Domain.Services;

public interface IEncryptionService
{
	string Encrypt(string plainText, string key, out byte[] iv);
	string Decrypt(string cipherText, string key, byte[] iv);
	byte[] IV(string text);
	string Text(string text);
}
