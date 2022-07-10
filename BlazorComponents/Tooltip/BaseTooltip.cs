using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public class BaseTooltip : ComponentBase
	{
		#region Parameters

		[Parameter] public string TooltipText { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public TooltipDirection Direction { get; set; } = TooltipDirection.Up;

		#endregion

		#region Properties and Fields

		protected string Class { get; set; }

		#endregion

		#region Methods

		protected override void OnParametersSet()
		{
			Class = Direction switch
			{
				TooltipDirection.Up => "tooltip-up",
				TooltipDirection.Down => "tooltip-down",
				TooltipDirection.Left => "tooltip-left",
				TooltipDirection.Right => "tooltip-right",
				_ => string.Empty
			};
		}

		#endregion
	}
}
