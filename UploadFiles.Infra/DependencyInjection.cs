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
		services.AddScoped<IEncryptionService, EncryptionService>();
		services.AddScoped<IGenerateKeyService, GenerateKeyService>();
		services.AddScoped<IValidateLogin, ValidateLogin>();
		services.AddScoped<IGetTokenService, GetTokenService>();

		services.AddScoped<IUnitOfWorks, UnitOfWorks>();

		services.AddSingleton<IEncryptionSettingsService, EncryptionSettingsService>();

		#region Enums
		services.AddKeyedScoped<IEnunsService, BytesEnumServices>("BytesEnum");
		#endregion

		return services;
	}
}
