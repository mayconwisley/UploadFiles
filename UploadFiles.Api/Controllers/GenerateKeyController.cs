using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.GenerateKey;
using UploadFiles.App.Dtos.GenerateKey.Enum;
using UploadFiles.App.UseCases.GenerateKey.GetKey;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.Api.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Produces("application/json")]
	public class GenerateKeyController(IMediator _mediator) : ControllerBase
	{

		[HttpGet("{bytes}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerateKeyDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(Error))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
		public async Task<IActionResult> GetKeyAsync(BytesEnum bytes = BytesEnum.Bytes32, CancellationToken cancellationToken = default)
		{
			var command = new Command(bytes);
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
			return Ok(result.Value.Key);
		}

	}
}
