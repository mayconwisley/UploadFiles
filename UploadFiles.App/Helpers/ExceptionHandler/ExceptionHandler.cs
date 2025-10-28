using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UploadFiles.Domain.Abstractions;

namespace UploadFiles.App.Helpers.ExceptionHandler;

public static class ExceptionHandler
{
    public static async Task<Result<T>> TryAsync<T>(Func<CancellationToken, Task<Result<T>>> action, CancellationToken cancellationToken = default)
    {
        try
        {
            return await action(cancellationToken);
        }
        catch (DbUpdateException dbEx)
        {
            return Result.Failure<T>(Error.InternalServer($"Erro no banco de dados: {dbEx.InnerException?.Message ?? dbEx.Message}"));
        }
        catch (ValidationException valEx)
        {
            return Result.Failure<T>(Error.Validation(valEx.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(Error.InternalServer($"Erro inesperado: {ex.Message}"));
        }
    }
}
