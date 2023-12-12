using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorComponents
{
	public partial class WizardStep
	{
		[CascadingParameter] protected internal Wizard Parent { get; set; }

		[Parameter] public RenderFragment StepContent { get; set; }
		[Parameter] public RenderFragment BackwardsButtonContent { get; set; }
		[Parameter] public RenderFragment ForwardsButtonContent { get; set; }
		[Parameter] public EventCallback OnFinalActionCallback { get; set; }
		[Parameter] public EventCallback OnStepViewCallback { get; set; }
		[Parameter] public bool DisableForwardButton { get; set; } = false;
		[Parameter] public Colour NavIconColour { get; set; } = Colour.Black;
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		protected override void OnInitialized()
		{
			Parent.AddStep(this);
		}

		public void OnFinalAction() => OnFinalActionCallback.InvokeAsync();
	}
}
