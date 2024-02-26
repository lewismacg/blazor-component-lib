using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class NotificationsWidget
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public EventCallback OnViewAllCallback { get; set; }

		#endregion
	}
}
