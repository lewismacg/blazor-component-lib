using BlazorComponents;

namespace ComponentDemo.Models
{
	public class CategorisedSelectModel
	{
		public CategorisedSelectModel(NotificationType notificationType, string value)
		{
			NotificationType = notificationType;
			Value = value;
		}

		public NotificationType NotificationType { get; set; }
		public string Value { get; set; }
	}
}
