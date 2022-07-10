using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public class CardBodyBase : ComponentBase
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// An extra class to apply to the card body.
		/// </summary>
		[Parameter] public string ClassOverride { get; set; } = string.Empty;

		#endregion
	}
}
