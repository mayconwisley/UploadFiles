using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.Token;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Services;

namespace UploadFiles.App.UseCases.Login.Sign;


public sealed class Handler(IGetTokenService _getTokenService) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			var dto = command.LoginDto;
			if (dto is null)
				return Result.Failure<Response>(Error.BadRequest("Dados inválidos para o cadastro de usuário"));

			var token = await _getTokenService.Token(dto.ToUser());
			if (token.IsFailure)
				return Result.Failure<Response>(Error.BadRequest("Erro Gerar Token"));
			var tokenDto = new TokenDto(token.Value);


			return Result.Success(new Response(tokenDto));
		}, cancellationToken);
	}
}
