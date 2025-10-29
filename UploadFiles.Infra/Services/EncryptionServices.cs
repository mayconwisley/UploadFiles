using System.Security.Cryptography;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public class EncryptionServices : IEncryptionServices
{
	public string Decrypt(string cipherText, string key, byte[] iv)
	{
		byte[] buffer = Convert.FromBase64String(cipherText);
		byte[] keyBytes = Convert.FromBase64String(key);

		using Aes aes = Aes.Create();
		aes.Key = keyBytes;
		aes.IV = iv;

		ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

		using MemoryStream memoryStream = new(buffer);
		using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
		using StreamReader streamReader = new(cryptoStream);
		return streamReader.ReadToEnd();
	}

	public string Encrypt(string plainText, string key, out byte[] iv)
	{
		iv = new byte[32]; // IV de 32 bytes
		byte[] keyBytes = Convert.FromBase64String(key);

		// Gera o IV aleatório
		using Aes aes = Aes.Create();
		aes.GenerateIV();
		iv = aes.IV;
		aes.Key = keyBytes;

		ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

		using MemoryStream memoryStream = new();
		using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
		using (StreamWriter streamWriter = new(cryptoStream))
		{
			streamWriter.Write(plainText);
		}
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public byte[] IV(string text)
	{
		var strText = text.Split(':');

		if (strText.Length != 2)
		{
			throw new Exception("Campo inválido no banco");
		}

		return Convert.FromBase64String(strText[0]);
	}

	public string Text(string text)
	{
		var strText = text.Split(':');

		if (strText.Length != 2)
		{
			throw new Exception("Campo inválido no banco");
		}

		return strText[1];
	}
}
