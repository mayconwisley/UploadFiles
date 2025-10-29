using System.ComponentModel;

namespace UploadFiles.App.Helpers;

public static class EnumExtensions
{
	public static string GetDescription(this Enum value)
	{
		var field = value.GetType().GetField(value.ToString());
		var attr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
						.Cast<DescriptionAttribute>()
						.FirstOrDefault();
		return attr?.Description ?? value.ToString();
	}
}
