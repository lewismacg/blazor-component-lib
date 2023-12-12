using System;

namespace BlazorComponents
{
	public class ToastModel
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public NotificationType Level { get; set; } = NotificationType.Blue;
		public int AutoHideMilliseconds { get; set; }
		public DateTime Time { get; set; }
	}
}
