using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class BaseTooltip : ComponentBase, IAsyncDisposable
	{
		#region Parameters

		[Parameter] public string TooltipText { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public TooltipDirection Direction { get; set; } = TooltipDirection.Up;

		#endregion

		#region Dependencies

		[Inject] public IJSRuntime JsRuntime { get; set; }

		#endregion

		#region Properties and Fields

		protected bool Show { get; set; }
		protected ElementReference Origin { get; set; }
		protected string Id { get; set; } = Guid.NewGuid().ToString();
		protected IJSObjectReference Popper { get; set; }

		#endregion

		#region Methods

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var direction = Direction switch
				{
					TooltipDirection.Up => "top",
					TooltipDirection.Down => "bottom",
					TooltipDirection.Left => "left",
					TooltipDirection.Right => "right",
					_ => string.Empty
				};

				Popper = await JsRuntime.InvokeAsync<IJSObjectReference>("Popover.createPopover", Origin, Id, direction);
			}
		}

		/// <summary>
		/// The js interop is required for DOM updates that popper is unaware of. I.e. elements hidden behind a blazor if statement that then appears - without
		/// the update call popper will not realign the tooltip. 
		/// </summary>
		/// <param name="show"></param>
		/// <returns></returns>
		protected async Task ToggleVisibility(bool show)
		{
			Show = show;

			if (Show && Popper != null) await Popper.InvokeVoidAsync("update");
		}

		public async ValueTask DisposeAsync()
		{
			if (Popper != null)
			{
				await Popper.InvokeVoidAsync("destroy");
				await Popper.DisposeAsync();
			}
		}

		#endregion
	}
}
