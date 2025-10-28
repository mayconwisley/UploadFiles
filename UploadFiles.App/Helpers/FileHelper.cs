using Microsoft.AspNetCore.Http;

namespace UploadFiles.App.Helpers;

public static class FileHelper
{
    public static async Task<Stream> ToStreamAsync(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}
