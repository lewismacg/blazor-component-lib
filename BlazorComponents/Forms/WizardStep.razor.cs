using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class WizardStep
	{
		#region Parameters

		[CascadingParameter] protected internal Wizard Parent { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string BackwardsButtonText { get; set; } = "Previous";
		[Parameter] public string ForwardButtonText { get; set; } = "Next";
		[Parameter] public bool ShowIcons { get; set; }
		[Parameter] public EventCallback OnFinalActionCallback { get; set; }
		[Parameter] public EventCallback OnStepViewCallback { get; set; }
		[Parameter] public string ExtraButtonClasses { get; set; } = "";
		[Parameter] public bool DisableForwardButton { get; set; } = false;

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			Parent.AddStep(this);
		}

		public void OnFinalAction() => OnFinalActionCallback.InvokeAsync();

		#endregion

	}
}
