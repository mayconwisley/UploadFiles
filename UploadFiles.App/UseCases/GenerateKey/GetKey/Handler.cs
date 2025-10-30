using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.GenerateKey;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Services;

namespace UploadFiles.App.UseCases.GenerateKey.GetKey;

public sealed class Handler(IGenerateKeyService _generateKeyServices) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			var getKey = await _generateKeyServices.GetGenerateKey();
			if (getKey is null)
				return Result.Failure<Response>(Error.NullValue("Sem dados para exibir"));
			var key = new GenerateKeyDto(getKey);
			return Result.Success(new Response(key));
		}, cancellationToken);
	}
}
