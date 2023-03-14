using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class BaseMultiSelect<TValue> : ComponentBase
	{
		#region Parameters

		[Parameter] public EventCallback<List<TValue>> ValueChanged { get; set; }
		[Parameter] public List<TValue> Options { get; set; } = new();
		[Parameter] public List<TValue> SelectedValues { get; set; } = new();
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public string DateTimeStringFormat { get; set; } = "dd/MM/yyyy";
		[Parameter] public bool IsSmall { get; set; }
		[Parameter] public bool AllowSearch { get; set; }
		[Parameter] public string Placeholder { get; set; } = "Choose values from list...";

		#endregion

		#region Properties and Fields

		protected string Value => string.Join(", ", GetStringRepresentation(SelectedValues));
		protected bool IsActive { get; set; }
		protected List<TValue> FilteredOptions { get; set; } = new();
		protected bool SearchFocused { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			FilteredOptions = Options;
			base.OnInitialized();
		}

		protected async Task OnClick(TValue value)
		{
			if (!SelectedValues.Contains(value)) SelectedValues.Add(value);
			else SelectedValues.Remove(value);

			await ValueChanged.InvokeAsync(SelectedValues);
		}

		protected async Task SetDisplayState(bool display)
		{
			if (SearchFocused) return;

			IsActive = display;
			if (!IsActive) await OnClose.InvokeAsync();
		}

		private List<string> GetStringRepresentation(List<TValue> selectedValues)
		{
			return selectedValues.Select(GetStringRepresentation).ToList();
		}

		protected string GetStringRepresentation(TValue option)
		{
			return option switch
			{
				DateTime dateTime => dateTime.ToString(DateTimeStringFormat),
				_ => option.ToString()
			};
		}

		protected void FilterList(ChangeEventArgs e)
		{
			var filterValue = e.Value?.ToString();
			FilteredOptions = string.IsNullOrWhiteSpace(filterValue) ? Options : Options.Where(x => GetStringRepresentation(x).ToUpper().Contains(filterValue.ToUpper())).ToList();
		}

		protected void SetFocus() => SearchFocused = true;
		protected void RemoveFocus() => SearchFocused = false;

		#endregion

	}
}