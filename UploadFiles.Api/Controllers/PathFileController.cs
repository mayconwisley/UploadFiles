using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.PathFile;
using UploadFiles.Domain.Abstractions;
using CreatePathFileCommand = UploadFiles.App.UseCases.PathFile.Create.Command;
using DeletePathFileCommand = UploadFiles.App.UseCases.PathFile.Delete.Command;
using GetByIdPathFileCommand = UploadFiles.App.UseCases.PathFile.GetById.Command;
using GetPathFileCommand = UploadFiles.App.UseCases.PathFile.Get.Command;
using UpdatePathFileCommand = UploadFiles.App.UseCases.PathFile.Update.Command;


namespace UploadFiles.Api.Controllers;


[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class PathFileController(IMediator _mediator) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PathFileOutputDto))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var command = new GetPathFileCommand();
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
		return Ok(result.Value.PathFileOutputDto);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PathFileOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> CreateAsync([FromBody] PathFileCreateDto familyCreateDto, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
		{
			var errorMessage = ErrorModelState();
			var error = Result.Failure(Error.BadRequest($"Erro de validação no objeto ({nameof(PathFileCreateDto)}): {errorMessage}"));
			return BadRequest(error.Error);
		}

		var command = new CreatePathFileCommand(familyCreateDto);
		var result = await _mediator.SendAsync(command, cancellationToken);

		if (result.IsFailure)
		{
			return result.Error.StatusCode switch
			{
				HttpStatusCode.BadRequest => BadRequest(result.Error),
				_ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
			};
		}

		return CreatedAtRoute("GetByPathFileId", new { id = result.Value.PathFileOutputDto.Id }, result.Value.PathFileOutputDto);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PathFileOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> UpdateAsync([FromBody] PathFileUpdateDto pathFileUpdateDto, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
		{
			var errorMessage = ErrorModelState();
			var error = Result.Failure(Error.BadRequest($"Erro de validação no objeto ({nameof(PathFileOutputDto)}): {errorMessage}"));
			return BadRequest(error.Error);
		}

		var command = new UpdatePathFileCommand(pathFileUpdateDto);
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

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PathFileOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> RemoveCoatOfArms(Guid id, CancellationToken cancellationToken = default)
	{
		if (id == Guid.Empty)
		{
			var error = Result.Failure(Error.BadRequest($"Id inválido ou vazio: {id}"));
			return BadRequest(error.Error);
		}

		var command = new DeletePathFileCommand(id);
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

	[HttpGet("{id}", Name = "GetByPathFileId")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PathFileOutputDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
	public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		if (id == Guid.Empty)
		{
			var error = Result.Failure(Error.BadRequest($"Id inválido ou vazio"));
			return BadRequest(error.Error);
		}
		var command = new GetByIdPathFileCommand(id);
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

	private string ErrorModelState()
	{
		var errorMessage = string.Join("; ", ModelState.Values
											  .SelectMany(x => x.Errors)
											  .Select(x => x.ErrorMessage));
		return errorMessage;
	}
}
