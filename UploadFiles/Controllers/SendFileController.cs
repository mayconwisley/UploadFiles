using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UploadFiles.App.Abstractions.Mediator;
using UploadFiles.App.Dtos.UploadFile;
using UploadFiles.App.Helpers;
using UploadFiles.App.UseCases.PathFile.SaveFile;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.Api.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SendFileController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
    public async Task<IActionResult> UploadCoatOfArms([FromForm] UploadFileDto uploadFile, CancellationToken cancellationToken = default)
    {
        var file = uploadFile.FormFile;
        var fileInfo = new FileInfo(file.FileName);

        if (file.Length == 0)
        {
            var error = Result.Failure(Error.BadRequest($"Arquivo inválido."));
            return BadRequest(error.Error);
        }

        var pathFile = await FileHelper.ToStreamAsync(uploadFile.FormFile);

        var command = new Command(pathFile, fileInfo.Name);
        var result = await _mediator.SendAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result.Error),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            };
        }

        return Ok();
    }
}
