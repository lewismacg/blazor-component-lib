using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public class DrawerBase : ComponentBase
	{
		#region Parameters

		[Parameter] public bool ExpansionState { get; set; } = true;
		[Parameter] public string Brand { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }

		#endregion

		#region Methods

		public string GetExpansionIcon()
		{
			return ExpansionState ? "subdirectory_arrow_left" : "subdirectory_arrow_right";
		}

		public void ToggleDrawerExpansion()
		{
			ExpansionState = !ExpansionState;
			StateHasChanged();
		}

		#endregion

	}
}
