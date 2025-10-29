using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.User.GetByUsername;

public sealed class Handler(IUserRepository _userRepository) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			if (string.IsNullOrEmpty(command.Username))
				return Result.Failure<Response>(Error.Validation("Usuário inválido"));

			var getEntity = await _userRepository.GetByUsernameAsync(command.Username, cancellationToken);
			if (getEntity is null)
				return Result.Failure<Response>(Error.NullValue("Sem dados para exibir"));

			return Result.Success(new Response(getEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
