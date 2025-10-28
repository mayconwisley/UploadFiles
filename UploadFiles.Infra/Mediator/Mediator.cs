using UploadFiles.App.Abstractions.Mediator;

namespace UploadFiles.Infra.Mediator;

public sealed class Mediator(IServiceProvider _serviceProvider) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>)
             .MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType) ??
            throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

        return await (Task<TResponse>)handlerType
            .GetMethod("HandlerAsync")!
            .Invoke(handler, [request, cancellationToken])!;
    }
}
