using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BlazorComponents
{
	public partial class ButtonTabControl
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string ActiveTabClass { get; set; } = "alert-green";
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		#endregion
		#region Properties

		public ButtonTab ActiveTab { get; set; }
		private List<ButtonTab> _tabs { get; set; } = new();

		#endregion

		#region Methods

		internal void AddTab(ButtonTab tab)
		{
			_tabs.Add(tab);

			if (_tabs.Count == 1) tab.ActivateTab();
			StateHasChanged();
		}

		internal void RemoveTab(ButtonTab tab)
		{
			if (ActiveTab == tab) RemoveActiveArea();
			else
			{
				_tabs.Remove(tab);
				StateHasChanged();
			}
		}

		public void RemoveActiveArea()
		{
			var pageIndex = _tabs.IndexOf(ActiveTab);

			_tabs.Remove(ActiveTab);

			if (_tabs.Any())
			{
				if (pageIndex < _tabs.Count) _tabs[pageIndex].ActivateTab();
				else if (pageIndex - 1 < _tabs.Count) _tabs[pageIndex - 1].ActivateTab();
				else _tabs.First().ActivateTab();
			}

			StateHasChanged();
		}

		#endregion
	}
}