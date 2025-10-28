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
        services.AddScoped<IUploadFileStorageService, UploadFileStorageService>();
        services.AddScoped<IUnitOfWorks, UnitOfWorks>();

        return services;
    }
}
