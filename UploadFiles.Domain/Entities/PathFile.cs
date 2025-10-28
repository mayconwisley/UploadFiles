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
        Path = path;
    }

    public Result Update(string path)
    {
        Path = path;
        return Result.Success();
    }
}
