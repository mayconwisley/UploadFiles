using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UploadFiles.Infra.Database;

public class UploadFilesDbContextFactory : IDesignTimeDbContextFactory<UploadFilesDbContext>
{
    public UploadFilesDbContext CreateDbContext(string[] args)
    {
        var connection = UploadFilesDbConnection.SQL_CONNECTION;
        var optionBuilder = new DbContextOptionsBuilder<UploadFilesDbContext>();
        optionBuilder.UseSqlite(connection);
        return new UploadFilesDbContext(optionBuilder.Options);
    }
}
