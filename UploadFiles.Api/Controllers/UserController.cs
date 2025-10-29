using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.User;
using UploadFiles.Domain.Abstractions;
using CreateUserCommand = UploadFiles.App.UseCases.User.Create.Command;
using DeleteUserCommand = UploadFiles.App.UseCases.User.Delete.Command;
using GetAllByIdUserCommand = UploadFiles.App.UseCases.User.GetById.Command;
using GetAllUserCommand = UploadFiles.App.UseCases.User.GetAll.Command;
using GetByUsernameUserCommand = UploadFiles.App.UseCases.User.GetByUsername.Command;
using UpdateUserCommand = UploadFiles.App.UseCases.User.Update.Command;

namespace UploadFiles.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class UserController(IMediator _mediator) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserOutputDto>))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var command = new GetAllUserCommand();
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
		return Ok(result.Value.UserOutputDtos);
	}
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto userCreateDto, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
		{
			var errorMessage = ErrorModelState();
			var error = Result.Failure(Error.BadRequest($"Erro de validação no objeto ({nameof(UserOutputDto)}): {errorMessage}"));
			return BadRequest(error.Error);
		}

		var command = new CreateUserCommand(userCreateDto);
		var result = await _mediator.SendAsync(command, cancellationToken);

		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}

		return CreatedAtRoute("GetByUserId", new { id = result.Value.UserOutputDto.Id }, result.Value.UserOutputDto);
	}
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
		{
			var errorMessage = ErrorModelState();
			var error = Result.Failure(Error.BadRequest($"Erro de validação no objeto ({nameof(UserOutputDto)}): {errorMessage}"));
			return BadRequest(error.Error);
		}

		var command = new UpdateUserCommand(userUpdateDto);
		var result = await _mediator.SendAsync(command, cancellationToken);
		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}

		return Ok(result.Value);
	}
	[HttpGet("username/{username}", Name = "GetByUsername")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> GetByIdAsync(string username, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrEmpty(username))
		{
			var error = Result.Failure(Error.BadRequest($"Usuário inválido ou vazio"));
			return BadRequest(error.Error);
		}
		var command = new GetByUsernameUserCommand(username);
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
		return Ok(result.Value);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> RemoveCoatOfArms(Guid id, CancellationToken cancellationToken = default)
	{
		if (id == Guid.Empty)
		{
			var error = Result.Failure(Error.BadRequest($"Id inválido ou vazio: {id}"));
			return BadRequest(error.Error);
		}

		var command = new DeleteUserCommand(id);
		var result = await _mediator.SendAsync(command, cancellationToken);

		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				HttpStatusCode.NotFound => NotFound(result.Error),
				HttpStatusCode.UnprocessableEntity => UnprocessableEntity(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}

		return Ok(result.Value);
	}
	[HttpGet("{id}", Name = "GetByUserId")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		if (id == Guid.Empty)
		{
			var error = Result.Failure(Error.BadRequest($"Id inválido ou vazio"));
			return BadRequest(error.Error);
		}
		var command = new GetAllByIdUserCommand(id);
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
		return Ok(result.Value);
	}
	private string ErrorModelState()
	{
		var errorMessage = string.Join("; ", ModelState.Values
											  .SelectMany(x => x.Errors)
											  .Select(x => x.ErrorMessage));
		return errorMessage;
	}
}
