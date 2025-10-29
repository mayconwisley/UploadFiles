using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;

namespace UploadFiles.App.UseCases.User.Update;

public sealed class Handler(IUserRepository _userRepository, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			var dto = command.UserUpdateDto;
			if (dto is null)
				return Result.Failure<Response>(Error.BadRequest("Dados inválidos para a atualização do usuário"));

			var getEntity = await _userRepository.GetByIdAsync(dto.Id);
			if (getEntity is null)
				return Result.Failure<Response>(Error.BadRequest($"Dados não encontrado para o id {dto.Id}"));

			var updateEntity = getEntity.Update(
				dto.Username,
				dto.Password
			);

			if (updateEntity.IsFailure)
				return Result.Failure<Response>(updateEntity.Error);

			await _unitOfWorks.CommitAsync();

			return Result.Success(new Response(getEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
