using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.PathFile.Update;

public sealed class Handler(IPathFileRepository _pathFileRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            var dto = command.PathFileUpdateDto;
            if (dto is null)
                return Result.Failure<Response>(Error.Validation("Dados inválidos para a atualização do local do arquivo"));

            var updateEntity = await _pathFileRepository.UpdateAsync(dto.ToPathFileUpdate(), cancellationToken);
            await _unitOfWorks.CommitAsync();

            return Result.Success(new Response(updateEntity.ToPathFileOutputDto()));
        }, cancellationToken);
    }
}
