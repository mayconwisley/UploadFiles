using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.PathFile.Create;

public sealed class Handler(IPathFileRepository _pathFileRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            var dto = command.PathFileCreateDto;

            var isContens = await _pathFileRepository.GetAsync();
            if (isContens is not null)
                return Result.Failure<Response>(Error.BadRequest("Já existe um caminho cadastrado"));

            if (dto is null)
                return Result.Failure<Response>(Error.BadRequest("Dados inválidos para a criação do local do arquivo"));

            var saveEntity = await _pathFileRepository.CreateAsync(dto.ToPathFileCreate(), cancellationToken);
            await _unitOfWorks.CommitAsync();

            return Result.Success(new Response(saveEntity.ToPathFileOutputDto()));
        }, cancellationToken);
    }
}
