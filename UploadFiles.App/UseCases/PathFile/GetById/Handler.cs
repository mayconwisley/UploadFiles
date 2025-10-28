using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.PathFile.GetById;

public sealed class Handler(IPathFileRepository _pathFileRepository) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            if (command.Id == Guid.Empty)
                return Result.Failure<Response>(Error.Validation("Id inválidos para a consultar local do arquivo"));

            var getEntity = await _pathFileRepository.GetByIdAsync(command.Id, cancellationToken);
            if (getEntity is null)
                return Result.Failure<Response>(Error.NullValue("Sem dados para exibir"));

            return Result.Success(new Response(getEntity.ToPathFileOutputDto()));
        }, cancellationToken);
    }
}
