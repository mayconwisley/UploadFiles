using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.User.Create;

public sealed class Handler(IUserRepository _userRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			var dto = command.UserCreateDto;

			if (dto is null)
				return Result.Failure<Response>(Error.BadRequest("Dados inválidos para o cadastro de usuário"));

			var saveEntity = await _userRepository.CreateAsync(dto.ToUserCreate(), cancellationToken);
			await _unitOfWorks.CommitAsync();

			return Result.Success(new Response(saveEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
