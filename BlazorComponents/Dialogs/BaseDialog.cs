using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class BaseDialog : ComponentBase
	{
		#region Parameters

		[Parameter] public bool ShowDialog { get; set; }
		[Parameter] public bool Large { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string DialogTitle { get; set; }
		[Parameter] public string AdditionalClass { get; set; }
		[Parameter] public string DialogId { get; set; } = Guid.NewGuid().ToString();
		[Parameter] public bool IsDraggable { get; set; }

		#endregion

		#region Dependencies

		[Inject] private IJSRuntime Runtime { get; set; }

		#endregion

		#region Properties and Fields

		protected string DialogHandleId => $"{DialogId}Handle";

		#endregion

		#region Methods

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (!IsDraggable) return base.OnAfterRenderAsync(firstRender);

			if (ShowDialog) Runtime.InvokeVoidAsync("DraggableElement.makeDraggable", DialogId, DialogHandleId);

			return base.OnAfterRenderAsync(firstRender);
		}

		#endregion

	}
}
