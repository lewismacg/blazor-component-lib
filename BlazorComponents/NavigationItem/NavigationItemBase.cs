using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public class NavigationItemBase : ComponentBase
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public bool HasChildren { get; set; }
		[Parameter] public string Icon { get; set; }
		[Parameter] public string IconTitle { get; set; }
		[Parameter] public string LinkText { get; set; }
		[Parameter] public string Destination { get; set; }
		[Parameter] public bool ForceReload { get; set; }
		[Parameter] public string Id { get; set; }

		#endregion

		#region Dependencies

		[Inject] public NavigationManager NavManager { get; set; }

		#endregion

		#region Methods

		public void Navigate()
		{
			if (!HasChildren) NavManager?.NavigateTo(Destination, ForceReload);
		}

		#endregion
	}
}
