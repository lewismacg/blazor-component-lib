using System;

namespace BlazorComponents
{
	public class ToasterManager
	{
		public event Action<ToastModel> ToastAdded;

		public void AddTimedToast(string toastTitle, string toastBody, NotificationType toastLevel, DateTime toastAddedTime, int hideTimeMilliseconds)
		{
			ToastAdded?.Invoke(new ToastModel { Title = toastTitle, Body = toastBody, Level = toastLevel, Time = toastAddedTime, AutoHideMilliseconds = hideTimeMilliseconds });
		}

		public void AddToast(string toastTitle, string toastBody, NotificationType toastLevel, DateTime? toastAddedTime = null)
		{
			var hideTimeMilliseconds = GetAutoHideMillisecondsFromToastLevel(toastLevel);
			ToastAdded?.Invoke(new ToastModel { Title = toastTitle, Body = toastBody, Level = toastLevel, Time = toastAddedTime ?? DateTime.Now, AutoHideMilliseconds = hideTimeMilliseconds });
		}

		private int GetAutoHideMillisecondsFromToastLevel(NotificationType toastLevel)
		{
			return toastLevel switch
			{
				NotificationType.Blue => 5000,
				NotificationType.Green => 10000,
				NotificationType.Yellow => 20000,
				NotificationType.Red => 90000,
				NotificationType.None => 5000,
				_ => throw new ArgumentOutOfRangeException(nameof(toastLevel), toastLevel, null)
			};
		}
	}
}
