namespace UploadFiles.App.Abstractions.Mediator
{
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandlerAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
