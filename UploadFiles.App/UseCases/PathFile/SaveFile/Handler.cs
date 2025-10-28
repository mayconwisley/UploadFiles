using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Services;

namespace UploadFiles.App.UseCases.PathFile.SaveFile;

public sealed class Handler(IUploadFileStorageService _uploadFileStorageService) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
    {
        return await ExceptionHandler.TryAsync(async ct =>
        {
            var getEntity = await _uploadFileStorageService.SaveFileAsync(command.FileStream, command.Name);
            if (!getEntity)
                return Result.Failure<Response>(Error.NullValue("Arquino não foi salvo com sucesso"));

            return Result.Success(new Response(getEntity));
        }, cancellationToken);
    }
}
