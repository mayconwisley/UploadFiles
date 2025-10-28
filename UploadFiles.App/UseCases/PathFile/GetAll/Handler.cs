using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.PathFile.GetAll;

public sealed class Handler(IPathFileRepository _pathFileRepository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            var getEntity = await _pathFileRepository.GetAsync(cancellationToken);
            if (getEntity is null)
            {
                return Result.Failure<Response>(Error.BadRequest("Sem dados para listar"));
            }
            return Result.Success(new Response(getEntity.ToPathFileOutputDto()));

        }, cancellationToken);
    }
}
