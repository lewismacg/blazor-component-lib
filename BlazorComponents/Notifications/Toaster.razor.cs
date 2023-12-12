using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BlazorComponents
{
	public partial class Toaster : ComponentBase
	{
		#region Parameters

		[CascadingParameter] public ToasterManager ToasterManager { get; set; }

		#endregion

		#region Properties and Fields

		private List<ToastModel> Toasts { get; set; } = new();

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			ToasterManager.ToastAdded += ShowToast;
		}

		private void ShowToast(ToastModel model)
		{
			InvokeAsync(() =>
			{
				Toasts.Add(model);

				StateHasChanged();
			});
		}

		public void RemoveToast(ToastModel model)
		{
			InvokeAsync(() =>
			{
				var toastInstance = Toasts.SingleOrDefault(x => x == model);

				if (toastInstance is null) return;

				Toasts.Remove(toastInstance);
				StateHasChanged();
			});
		}

		#endregion
	}
}
