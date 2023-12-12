using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class Popover : ComponentBase
	{
		#region Parameters

		[Parameter] public ElementReference Origin { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> UnknownParameters { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string Placement { get; set; } = "bottom-start";

		#endregion

		#region Dependencies

		[Inject] public IJSRuntime JsRuntime { get; set; }

		#endregion

		#region Properties and Fields

		private string Id { get; set; } = Guid.NewGuid().ToString();

		#endregion

		#region Methods

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender) await JsRuntime.InvokeVoidAsync("Popover.createPopover", Origin, Id, Placement);
		}

		#endregion
	}
}
