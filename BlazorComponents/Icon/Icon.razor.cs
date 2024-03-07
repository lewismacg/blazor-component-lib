using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class Icon : ComponentBase
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public Colour IconColour { get; set; } = Colour.Black;
		[Parameter] public bool IsLarge { get; set; }
		[Parameter] public EventCallback OnClickAction { get; set; }
		[Parameter] public string IconId { get; set; }

		#endregion
	}
}
