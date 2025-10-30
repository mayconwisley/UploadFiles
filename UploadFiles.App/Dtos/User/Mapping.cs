using UploadFiles.App.Dtos.Login;

namespace UploadFiles.App.Dtos.User;

public static class Mapping
{
	#region Output
	public static IEnumerable<UserOutputDto> ToListUserDto(this IEnumerable<Domain.Entities.User> users)
	{
		return [.. from user in users
				   select new UserOutputDto(
						user.Id,
						user.Username
				   )
		];
	}
	public static UserOutputDto ToUserOutputDto(this Domain.Entities.User userOutputDto)
		=> new(
			userOutputDto.Id,
			userOutputDto.Username
		);
	#endregion

	#region Create
	public static UserCreateDto ToUserCreateDto(this Domain.Entities.User user)
		=> new(
			user.Username,
			user.Password
		);

	public static Domain.Entities.User ToUserCreate(this UserCreateDto userCreateDto)
		=> new(
			userCreateDto.Username,
			userCreateDto.Password
		);
	#endregion

	#region Update
	public static Domain.Entities.User ToUserUpdate(this UserUpdateDto userUpdateDto)
		=> new(
			userUpdateDto.Id,
			userUpdateDto.Username,
			userUpdateDto.Password
		);
	#endregion

	#region Login
	public static Domain.Entities.User ToUser(this LoginDto loginDto)
		=> new(
			loginDto.Username,
			loginDto.Password
		);
	#endregion
}
