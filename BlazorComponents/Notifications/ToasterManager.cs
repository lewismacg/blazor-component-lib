using System;
using System.Collections.Generic;

namespace BlazorComponents
{
	public class ToasterManager
	{
		#region Propertie and Fields

		public List<ToastModel> Toasts = new();
		public event EventHandler ToastAdded;
		public event EventHandler ToastRemoved;

		#endregion

		#region Add Toast

		public void AddTimedToast(string toastTitle, string toastBody, NotificationType toastLevel, DateTime toastAddedTime, int hideTimeMilliseconds)
		{
			Toasts.Add(new ToastModel { Title = toastTitle, Body = toastBody, Level = toastLevel, Time = toastAddedTime, AutoHideSeconds = hideTimeMilliseconds });

			ToastAdded?.Invoke(this, EventArgs.Empty);
		}

		public void AddToast(string toastTitle, string toastBody, NotificationType toastLevel, DateTime? toastAddedTime = null)
		{
			var hideTimeMilliseconds = GetAutoHideMillisecondsFromToastLevel(toastLevel);
			Toasts.Add(new ToastModel { Title = toastTitle, Body = toastBody, Level = toastLevel, Time = toastAddedTime ?? DateTime.Now, AutoHideSeconds = hideTimeMilliseconds });

			ToastAdded?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Hide Toast

		private int GetAutoHideMillisecondsFromToastLevel(NotificationType toastLevel)
		{
			return toastLevel switch
			{
				NotificationType.Info => 5000,
				NotificationType.Success => 10000,
				NotificationType.Warning => 20000,
				NotificationType.Error => 90000,
				NotificationType.None => 5000,
				_ => throw new ArgumentOutOfRangeException(nameof(toastLevel), toastLevel, null)
			};
		}

		public void HideToast(ToastModel toast)
		{
			Toasts.Remove(toast);
			ToastRemoved?.Invoke(this, EventArgs.Empty);
		}

		#endregion

	}
}
