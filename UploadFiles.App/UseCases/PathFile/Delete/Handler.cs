using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.PathFile.Delete;

public sealed class Handler(IPathFileRepository _pathFileRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            if (command.Id == Guid.Empty)
                return Result.Failure<Response>(Error.Validation("Id inválidos para a exclusão do local do arquivo"));

            var deleteEntity = await _pathFileRepository.DeleteAsync(command.Id, cancellationToken);
            await _unitOfWorks.CommitAsync();

            return Result.Success(new Response(deleteEntity.ToPathFileOutputDto()));
        }, cancellationToken);
    }
}
