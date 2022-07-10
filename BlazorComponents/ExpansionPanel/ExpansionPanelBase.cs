using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public class ExpansionPanelBase : ComponentBase
	{
		#region Parameters

		[Parameter] public string Heading { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }

		protected bool _expansionState;
		[Parameter] public bool ExpansionState { get; set; }
		[Parameter] public bool ShowLine { get; set; } = true;

		#endregion

		#region Methods

		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender)
			{
				_expansionState = ExpansionState;
				StateHasChanged();
			}

			base.OnAfterRender(firstRender);
		}

		public string GetExpansionIcon()
		{
			return _expansionState ? "expand_less" : "expand_more";
		}

		public void ExpansionHeadingClicked()
		{
			_expansionState = !_expansionState;
			StateHasChanged();
		}

		#endregion

	}
}
