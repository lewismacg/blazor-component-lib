using System;
using System.Threading.Tasks;

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

		public async Task AddToastWithAction(Func<string, string, NotificationType, Task> action, string toastTitle, string toastBody, NotificationType toastLevel, DateTime? toastAddedTime = null)
		{
			AddToast(toastTitle, toastBody, toastLevel, toastAddedTime);
			await action.Invoke(toastTitle, toastBody, toastLevel);
		}

		public async Task AddToastWithCustomMessageAction(Func<Task> action, string toastTitle, string toastBody, NotificationType toastLevel, DateTime? toastAddedTime = null)
		{
			AddToast(toastTitle, toastBody, toastLevel, toastAddedTime);
			await action.Invoke();
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
