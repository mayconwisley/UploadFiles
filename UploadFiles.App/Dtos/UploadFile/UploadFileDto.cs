using Microsoft.AspNetCore.Http;

namespace UploadFiles.App.Dtos.UploadFile;

public record UploadFileDto(IFormFile FormFile);