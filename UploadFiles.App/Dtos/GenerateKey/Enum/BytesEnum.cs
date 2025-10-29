using System.ComponentModel;

namespace UploadFiles.App.Dtos.GenerateKey.Enum;

public enum BytesEnum
{
	[Description("16 Bytes")]
	Bytes16 = 16,

	[Description("24 Bytes")]
	Bytes24 = 24,

	[Description("32 Bytes")]
	Bytes32 = 32
}
