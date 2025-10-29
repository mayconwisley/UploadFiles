using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.App.Helpers.ExceptionHandler;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Repositories;
using UploadFiles.Domain.Services;

namespace UploadFiles.App.UseCases.User.Create;

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

			var dto = command.UserCreateDto;
			if (dto is null)
				return Result.Failure<Response>(Error.BadRequest("Dados inválidos para o cadastro de usuário"));

			var passwordEncryption = _encryptionServices.Encrypt(dto.Password, key, out byte[] bytePassword);
			var encryption = $"{Convert.ToBase64String(bytePassword)}:{passwordEncryption}";

			var user = new UserCreateDto(
				dto.Username,
				encryption
			);

			var saveEntity = await _userRepository.CreateAsync(user.ToUserCreate(), cancellationToken);
			await _unitOfWorks.CommitAsync();

			return Result.Success(new Response(saveEntity.ToUserOutputDto()));
		}, cancellationToken);
	}
}
