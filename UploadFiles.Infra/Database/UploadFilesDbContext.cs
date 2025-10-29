using Microsoft.EntityFrameworkCore;
using UploadFiles.Domain.Entities;

namespace UploadFiles.Infra.Database;

public class UploadFilesDbContext(DbContextOptions<UploadFilesDbContext> options) : DbContext(options)
{
	public DbSet<PathFile> PathFiles { get; set; } = null!;
	public DbSet<User> Users { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PathFile>()
			.HasKey(k => k.Id);

		modelBuilder.Entity<PathFile>()
			.HasIndex(i => i.Path)
			.IsUnique();

		modelBuilder.Entity<User>()
			.HasKey(k => k.Id);

		modelBuilder.Entity<User>()
			.HasIndex(i => i.Username)
			.IsUnique();
	}
}
