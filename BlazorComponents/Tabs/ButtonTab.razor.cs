using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorComponents
{
	public partial class ButtonTab : ComponentBase, IDisposable
	{
		#region Parameters

		[CascadingParameter] private ButtonTabControl Parent { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string TabTitle { get; set; }
		[Parameter] public bool InitialiseActive { get; set; }
		[Parameter] public bool AllowCaching { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		#endregion

		#region Properties and Fields

		private bool ChildContentRendered { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			if (Parent == null) throw new ArgumentNullException(nameof(Parent), "ButtonTab must be wrapped in ButtonTabControl component.");

			Parent.AddTab(this);
			if (InitialiseActive) ActivateTab();
		}

		public void ActivateTab()
		{
			ChildContentRendered = true;
			Parent.ActiveTab = this;
		}

		public void Dispose()
		{
			Parent.RemoveTab(this);
		}

		#endregion
	}
}