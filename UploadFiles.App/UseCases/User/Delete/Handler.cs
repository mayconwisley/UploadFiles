using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.User.Delete;

public sealed class Handler(IUserRepository _userRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			if (command.Id == Guid.Empty)
				return Result.Failure<Response>(Error.Validation("Id inválidos para a exclusão do usuário"));

			var deleteEntity = await _userRepository.DeleteAsync(command.Id, cancellationToken);
			await _unitOfWorks.CommitAsync();

			return Result.Success(new Response(deleteEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
