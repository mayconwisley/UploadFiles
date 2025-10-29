using Microsoft.Extensions.DependencyInjection;
using UploadFiles.App.Abstractions.Mediator;

namespace UploadFiles.App;

public static class DependencyInjection
{
	public static IServiceCollection AddApp(this IServiceCollection services)
	{
		var applicationAssembly = typeof(UseCases.PathFile.Create.Command).Assembly;



		var handlers = applicationAssembly.GetTypes()
			.Where(w => !w.IsAbstract && !w.IsInterface)
			.SelectMany(s => s.GetInterfaces()
				.Where(w => w.IsGenericType && w.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
				.Select(w => new { Handler = s, Interface = w })
			);

		foreach (var item in handlers)
		{
			services.AddScoped(item.Interface, item.Handler);
		}

		return services;
	}
}
