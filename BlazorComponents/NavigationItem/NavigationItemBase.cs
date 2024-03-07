using Microsoft.AspNetCore.Components;
using System;

namespace BlazorComponents
{
	public class NavigationItemBase : ComponentBase
	{
		#region Parameters

		[CascadingParameter] public NavigationParent Parent { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string Icon { get; set; }
		[Parameter] public string HoverTitle { get; set; }
		[Parameter] public string LinkText { get; set; }
		[Parameter] public string Destination { get; set; }
		[Parameter] public bool ForceReload { get; set; }

		#endregion

		#region Dependencies

		[Inject] public NavigationManager NavManager { get; set; }

		#endregion

		#region Properties and Fields

		public bool Show { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			if (string.IsNullOrWhiteSpace(Destination)) throw new Exception("A destination is required for a navigation item");

			if (Parent == null) Show = true;
			else Parent.AddNavItem(this);
		}

		public void Navigate()
		{
			NavManager?.NavigateTo(Destination, ForceReload);
		}

		#endregion
	}
}
