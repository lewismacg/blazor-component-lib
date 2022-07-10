using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorComponents
{
	public partial class Wizard
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public WizardStep ActiveStep { get; set; }
		[Parameter] public int ActiveStepIx { get; set; }
		[Parameter] public bool ShowNavAtTop { get; set; }
		[Parameter] public bool ShowNavAtBottom { get; set; }
		[Parameter] public bool HideFinalStep { get; set; }

		#endregion

		#region Properties and Fields

		protected internal List<WizardStep> Steps = new();
		public bool IsLastStep { get; set; }

		#endregion

		#region Methods

		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender)
			{
				SetActive(Steps[0]);
				StateHasChanged();
			}
		}

		protected internal void GoBack()
		{
			if (ActiveStepIx > 0) SetActive(Steps[ActiveStepIx - 1]);
		}

		protected internal void GoNext()
		{
			if (ActiveStepIx < Steps.Count - 1) SetActive(Steps[Steps.IndexOf(ActiveStep) + 1]);
		}

		protected internal void SetActive(WizardStep step)
		{
			ActiveStep = step ?? throw new ArgumentNullException(nameof(step));

			ActiveStep.OnStepViewCallback.InvokeAsync();

			ActiveStepIx = StepsIndex(step);

			if (ActiveStepIx == Steps.Count - 1) IsLastStep = true;
			else IsLastStep = false;
		}

		public int StepsIndex(WizardStep step) => StepsIndexInternal(step);
		protected int StepsIndexInternal(WizardStep step)
		{
			if (step == null) throw new ArgumentNullException(nameof(step));

			return Steps.IndexOf(step);
		}

		protected internal void AddStep(WizardStep step) => Steps.Add(step);

		#endregion

	}
}
