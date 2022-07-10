using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class ToastBase : ComponentBase
	{
		#region Parameters

		[Parameter] public DateTime Time { get; set; }
		[Parameter] public string Title { get; set; }
		[Parameter] public string Body { get; set; }
		[Parameter] public Action HideAction { get; set; }
		[Parameter] public int HideInSeconds { get; set; }
		[Parameter] public NotificationType Level { get; set; } = NotificationType.Info;

		#endregion

		#region Properties and Fields

		protected string IndicatorClass { get; set; }

		private bool _isInitialised;

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			base.OnInitialized();

			SetToastClass();
		}

		protected override void OnParametersSet()
		{
			if (!_isInitialised)
			{
				_isInitialised = true;
				SetAutoHide();
			}

			SetToastClass();

			base.OnParametersSet();
		}

		private void SetAutoHide()
		{
			if (HideInSeconds == default || HideInSeconds < 0) return;

			var autoHideTask = new Task(() =>
			{
				Thread.Sleep(HideInSeconds);
				CloseToast();
			});

			autoHideTask.Start();
		}

		private void SetToastClass()
		{
			IndicatorClass = string.Empty;

			if (Level == NotificationType.Error) IndicatorClass = "red-fill";
			if (Level == NotificationType.Warning) IndicatorClass = "yellow-fill";
			if (Level == NotificationType.Success) IndicatorClass = "green-fill";
			if (Level == NotificationType.Info) IndicatorClass = "blue-fill";
		}

		public string GetTimeString()
		{
			var now = DateTime.Now;
			var delta = now - Time;

			if (delta.TotalSeconds < 1) return "Just Now";
			if (delta.TotalSeconds < 60) return $"{delta.Seconds} secs ago";
			if (delta.TotalMinutes < 60) return $"{delta.Minutes} mins ago";
			if (delta.TotalHours < 24) return $"{delta.Hours} hrs ago";

			return "More than a day ago";
		}

		public void CloseToast()
		{
			HideAction?.Invoke();
		}

		#endregion

	}
}
