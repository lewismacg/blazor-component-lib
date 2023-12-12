using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class Toast : ComponentBase, IDisposable
	{
		#region Parameters

		[Parameter] public ToastModel Model { get; set; }
		[Parameter] public Action HideCallback { get; set; }

		#endregion

		#region Properties and Fields

		private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(1));

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			await RunTimer();
		}

		public void Close() => HideCallback.Invoke();

		public string GetFillColour()
		{
			return Model.Level switch
			{
				NotificationType.Red => "red-fill",
				NotificationType.Yellow => "yellow-fill",
				NotificationType.Green => "green-fill",
				NotificationType.Blue => "blue-fill",
				_ => string.Empty
			};
		}

		public string GetTimeString()
		{
			var timeSinceCreation = DateTime.Now - Model.Time;

			if (timeSinceCreation.TotalSeconds < 1) return "Just Now";
			if (timeSinceCreation.TotalSeconds < 60) return $"{timeSinceCreation.Seconds} secs ago";
			if (timeSinceCreation.TotalMinutes < 60) return $"{timeSinceCreation.Minutes} mins ago";
			if (timeSinceCreation.TotalHours < 24) return $"{timeSinceCreation.Hours} hrs ago";

			return "More than a day ago";
		}

		protected async Task RunTimer()
		{
			while (await _periodicTimer.WaitForNextTickAsync())
			{
				var totalMillisecondsThatHavePassedSinceCreation = (DateTime.Now - Model.Time).TotalMilliseconds;
				if (totalMillisecondsThatHavePassedSinceCreation >= Model.AutoHideMilliseconds) Close();
				await InvokeAsync(StateHasChanged);
			}
		}

		public void Dispose()
		{
			_periodicTimer.Dispose();
		}

		#endregion
	}
}
