namespace UploadFiles.Domain.Abstractions;

public interface IUnitOfWorks
{
    Task CommitAsync();
}
