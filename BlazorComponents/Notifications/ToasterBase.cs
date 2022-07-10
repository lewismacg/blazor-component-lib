using Microsoft.AspNetCore.Components;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class ToasterBase : ComponentBase
	{
		#region Parameters

		[CascadingParameter] public ToasterManager ToasterManager { get; set; }

		#endregion

		#region Properties and Fields

		protected ToastModel[] Toasts = new ToastModel[0];

		private bool _hasInitialised;
		private CancellationTokenSource tokenSource { get; set; } = new CancellationTokenSource();
		private CancellationToken timerCancellationToken { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			if (!_hasInitialised)
			{
				ToasterManager.ToastAdded += ToasterManger_ToastAdded;
				ToasterManager.ToastRemoved += ToasterManger_ToastRemoved;
				_hasInitialised = true;

				StartTimerUpdates();
			}
			base.OnInitialized();
		}

		private void ToasterManger_ToastRemoved(object sender, System.EventArgs e)
		{
			Toasts = ToasterManager.Toasts.ToArray();

			if (Toasts.Length == 0)
			{
				tokenSource.Cancel();
			}

			InvokeAsync(StateHasChanged);
		}

		private void ToasterManger_ToastAdded(object sender, System.EventArgs e)
		{
			if (Toasts.Length == 0)
			{
				StartTimerUpdates();
			}

			Toasts = ToasterManager.Toasts.ToArray();

			StateHasChanged();
		}

		public void HideToast(ToastModel toast)
		{
			ToasterManager.HideToast(toast);

			InvokeAsync(StateHasChanged);
		}

		private void StartTimerUpdates()
		{
			timerCancellationToken = tokenSource.Token;

			var timerTask = new Task(() =>
			{
				while (!timerCancellationToken.IsCancellationRequested)
				{
					Thread.Sleep(500);
					this.InvokeAsync(StateHasChanged);
				}
			}, timerCancellationToken);

			if (!timerCancellationToken.IsCancellationRequested) timerTask.Start();
		}

		#endregion

	}
}
