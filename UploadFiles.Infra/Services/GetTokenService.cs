using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Domain.Entities;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services;

public sealed class GetTokenService(IValidateLogin _validateLogin) : IGetTokenService
{
	private readonly IConfiguration _configuration = new ConfigurationBuilder()
		 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		 .Build();

	public async Task<Result<string>> Token(User user)
	{
		var validarLogin = await _validateLogin.IsValidateLogin(user.Username, user.Password);
		if (!validarLogin)
			return Result.Failure<string>(Error.BadRequest("Usuário ou Senha inválido"));


		string? strJWT = _configuration.GetSection("JWT")["Key"];
		string? issue = _configuration.GetSection("JWT")["Issuer"];
		string? audience = _configuration.GetSection("JWT")["Audience"];

		byte[] jwt = Encoding.UTF8.GetBytes(strJWT!);

		var tokenConfig = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(
			[
				new Claim("Acesso Api", "Acesso Api"),
				new Claim("Usuário", user.Username)
			]),
			Issuer = issue,
			Audience = audience,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwt),
																SecurityAlgorithms.HmacSha256Signature)
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenConfig);
		var strToken = tokenHandler.WriteToken(token);

		return Result.Success(strToken);
	}
}
