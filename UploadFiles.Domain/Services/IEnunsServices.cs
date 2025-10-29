using UploadFiles.Domain.Generics;

namespace UploadFiles.Domain.Services;

public interface IEnunsServices
{
	List<EnumsGeneric> GetAllEnums();
}
