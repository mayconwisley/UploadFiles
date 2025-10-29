using UploadFiles.Domain.Abstractions;

namespace UploadFiles.Domain.Entities;

public class PathFile
{
	public Guid Id { get; private set; }
	public string Path { get; private set; } = null!;
	protected PathFile() { }

	internal PathFile(Guid id, string path)
	{
		Id = id;
		Path = path;
	}

	public PathFile(string path)
	{
		if (string.IsNullOrEmpty(path))
			throw new ArgumentNullException(nameof(path), "Caminho da pasta esta vazio");

		Path = path;
	}

	public Result Update(string path)
	{
		if (string.IsNullOrEmpty(path))
			return Result.Failure(Error.BadRequest("Caminho da pasta esta vazio"));

		Path = path;
		return Result.Success();
	}
}
