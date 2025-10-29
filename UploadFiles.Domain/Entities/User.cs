using UploadFiles.Domain.Abstractions;

namespace UploadFiles.Domain.Entities;

public class User
{
	public Guid Id { get; private set; }
	public string Username { get; private set; } = null!;
	public string Password { get; private set; } = null!;

	protected User() { }

	internal User(Guid id, string username, string password)
	{
		Id = id;
		Username = username;
		Password = password;
	}

	public User(string username, string password)
	{
		Username = username;
		Password = password;
	}

	public Result Update(string username, string password)
	{
		if (string.IsNullOrEmpty(username))
			return Result.Failure(Error.BadRequest("Nome do usuário não pode ser vazio"));

		if (string.IsNullOrEmpty(password))
			return Result.Failure(Error.BadRequest("Password não pode ser vazio"));

		Username = username;
		Password = password;

		return Result.Success();
	}
}
