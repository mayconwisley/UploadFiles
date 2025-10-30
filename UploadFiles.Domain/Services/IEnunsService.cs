using UploadFiles.Domain.Generics;

namespace UploadFiles.Domain.Services;

public interface IEnunsService
{
	List<EnumsGeneric> GetAllEnums();
}
