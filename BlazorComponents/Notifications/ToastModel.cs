using System;

namespace BlazorComponents
{
	public class ToastModel
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public NotificationType Level { get; set; } = NotificationType.Info;
		public int AutoHideSeconds { get; set; }
		public DateTime Time { get; set; }
	}
}
