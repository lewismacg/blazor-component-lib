using Microsoft.AspNetCore.Components;
using System;

namespace BlazorComponents
{
	public class TabPageBase : ComponentBase, IDisposable
	{
		[CascadingParameter] public TabControl Parent { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string TabTitle { get; set; }
		[Parameter] public int Ordinal { get; set; }

		protected override void OnInitialized()
		{
			if (Parent == null) throw new ArgumentNullException(nameof(Parent), "TabPage must exist within a TabControl");
			Parent.AddPage(this);
		}

		public virtual void ActivatePage()
		{
			Parent.ActivePage = this;
		}

		public void Dispose()
		{
			Parent.RemovePage(this);
		}
	}
}