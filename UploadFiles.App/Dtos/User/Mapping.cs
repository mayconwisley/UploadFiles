namespace UploadFiles.App.Dtos.User;

public static class Mapping
{
	#region Output
	public static IEnumerable<UserOutputDto> ToListUserDto(this IEnumerable<Domain.Entities.User> users)
	{
		return [.. from user in users
				   select new UserOutputDto(
						user.Id,
						user.Username,
						user.Password
				   )
		];
	}
	public static IEnumerable<Domain.Entities.User> ToListUserDto(this IEnumerable<UserOutputDto> userOutputDtos)
		=> userOutputDtos.Select(s => s.ToUser());

	public static Domain.Entities.User ToUser(this UserOutputDto userOutputDto)
		=> new(
			userOutputDto.Id,
			userOutputDto.Username,
			userOutputDto.Password
		);

	public static UserOutputDto ToUserOutputDto(this Domain.Entities.User userOutputDto)
		=> new(
			userOutputDto.Id,
			userOutputDto.Username,
			userOutputDto.Password
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
}
