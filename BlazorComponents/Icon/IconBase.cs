using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace BlazorComponents
{
	public class IconBase : ComponentBase
	{
		#region Parameters

		[Parameter] public string IconName { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public Colour IconColour { get; set; } = Colour.Black;
		[Parameter] public bool IsLarge { get; set; } = false;
		[Parameter] public Action<MouseEventArgs> OnClickAction { get; set; } = null;
		[Parameter] public string IconId { get; set; }
		[Parameter] public string IconTitle { get; set; }

		#endregion

	}
}
