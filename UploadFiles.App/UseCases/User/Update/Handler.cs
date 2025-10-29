using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;
using UploadFiles.Domain.Services;

namespace UploadFiles.App.UseCases.User.Update;

public sealed class Handler(IUserRepository _userRepository, IEncryptionServices _encryptionServices,
	IEncryptionSettingsServices _encryptionSettingsServices, IUnitOfWorks _unitOfWorks) : IRequestHandler<Command, Result<Response>>
{
	public async Task<Result<Response>> HandlerAsync(Command command, CancellationToken cancellationToken)
	{
		return await ExceptionHandler.TryAsync(async ct =>
		{
			var key = _encryptionSettingsServices.Key;
			if (key is null)
				return Result.Failure<Response>(Error.BadRequest("Variavel de ambiente vazia"));

			var dto = command.UserUpdateDto;
			if (dto is null)
				return Result.Failure<Response>(Error.BadRequest("Dados inválidos para a atualização do usuário"));

			var passwordEncryption = _encryptionServices.Encrypt(dto.Password, key, out byte[] bytePassword);
			var encryption = $"{Convert.ToBase64String(bytePassword)}:{passwordEncryption}";

			var getEntity = await _userRepository.GetByIdAsync(dto.Id);
			if (getEntity is null)
				return Result.Failure<Response>(Error.BadRequest($"Dados não encontrado para o id {dto.Id}"));

			var updateEntity = getEntity.Update(
				dto.Username,
				encryption
			);

			if (updateEntity.IsFailure)
				return Result.Failure<Response>(updateEntity.Error);

			await _unitOfWorks.CommitAsync();

			return Result.Success(new Response(getEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
