using Microsoft.Extensions.DependencyInjection;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;
using UploadFiles.Domain.Services;
using UploadFiles.Infra.Database;
using UploadFiles.Infra.Repositories;
using UploadFiles.Infra.Services;

namespace UploadFiles.Infra;

public static class DependencyInjection
{
	public static IServiceCollection AddInfra(this IServiceCollection services)
	{
		services.AddScoped<IMediator, Mediator.Mediator>();
		services.AddScoped<IPathFileRepository, PathFileRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IUploadFileStorageService, UploadFileStorageService>();
		services.AddScoped<IEncryptionServices, EncryptionServices>();
		services.AddScoped<IGenerateKeyServices, GenerateKeyServices>();

		services.AddScoped<IUnitOfWorks, UnitOfWorks>();

		services.AddSingleton<IEncryptionSettingsServices, EncryptionSettingsServices>();

		#region Enums
		services.AddKeyedScoped<IEnunsServices, BytesEnumServices>("BytesEnum");
		#endregion

		return services;
	}
}
