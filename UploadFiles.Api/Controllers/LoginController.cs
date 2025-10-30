using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.Login;
using UploadFiles.App.Dtos.Token;
using UploadFiles.Domain.Abstractions;
using SetUserStandardCommand = UploadFiles.App.UseCases.Login.CreateUserStandard.Command;
using SignCommand = UploadFiles.App.UseCases.Login.Sign.Command;


namespace UploadFiles.Api.Controllers;
[ApiController]
[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]

public class LoginController(IMediator _mediator) : ControllerBase
{
	[HttpPost("setUserStandard")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> SetUserStandard(CancellationToken cancellationToken = default)
	{
		var command = new SetUserStandardCommand();
		var result = await _mediator.SendAsync(command, cancellationToken);
		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				HttpStatusCode.NotFound => NotFound(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}
		return Ok(result.Value.SetUserStandardDto);
	}
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken = default)
	{
		var command = new SignCommand(loginDto);
		var result = await _mediator.SendAsync(command, cancellationToken);
		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				HttpStatusCode.NotFound => NotFound(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}
		return Ok(result.Value.TokenDto);
	}
}
