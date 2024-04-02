using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class DefaultLayout : ComponentBase
	{
		#region Parameters

		[Parameter] public RenderFragment DropdownMenu { get; set; }
		[Parameter] public RenderFragment NotificationMenu { get; set; }
		[Parameter] public RenderFragment SearchBar { get; set; }
		[Parameter] public RenderFragment NavMenu { get; set; }
		[Parameter] public RenderFragment Content { get; set; }
		[Parameter] public bool FixNav { get; set; }

		#endregion

	}
}
