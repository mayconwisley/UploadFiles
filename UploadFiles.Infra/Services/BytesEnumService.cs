using UploadFiles.App.Dtos.GenerateKey.Enum;
using UploadFiles.App.Helpers;
using UploadFiles.Domain.Generics;
using UploadFiles.Domain.Services;

namespace UploadFiles.Infra.Services
{
	public class BytesEnumServices : IEnunsService
	{
		public List<EnumsGeneric> GetAllEnums()
		{
			return [.. Enum.GetValues<BytesEnum>()
					.Select(provider => new EnumsGeneric
					{
						Value = (int)provider,
						Nome = provider.GetDescription()
					})
	   ];
		}
	}
}
