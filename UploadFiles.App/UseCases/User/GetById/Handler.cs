using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.User.GetById;

public sealed class Handler(IUserRepository _userRepository) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			if (command.Id == Guid.Empty)
				return Result.Failure<Response>(Error.Validation("Id inválidos para a consultar o usuário"));

			var getEntity = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
			if (getEntity is null)
				return Result.Failure<Response>(Error.NullValue("Sem dados para exibir"));

			return Result.Success(new Response(getEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
