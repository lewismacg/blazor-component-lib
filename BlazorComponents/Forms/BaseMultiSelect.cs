using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class BaseMultiSelect<TValue> : ComponentBase where TValue : IConvertible
	{
		#region Parameters

		[Parameter] public EventCallback<List<TValue>> ValueChanged { get; set; }
		[Parameter] public List<TValue> Options { get; set; } = new();
		[Parameter] public string DateTimeStringFormat { get; set; } = "dd/MM/yyyy";
		[Parameter] public bool IsSmall { get; set; }

		#endregion

		#region Properties and Fields

		protected string Value { get; set; }
		protected bool IsActive { get; set; }
		public List<TValue> SelectedValues { get; set; } = new();

		#endregion

		#region Methods

		protected async Task OnClick(TValue value)
		{
			if (!SelectedValues.Contains(value)) SelectedValues.Add(value);
			else SelectedValues.Remove(value);

			Value = string.Join(", ", GetStringRepresentation(SelectedValues));

			await ValueChanged.InvokeAsync(SelectedValues);
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

		#endregion

	}
}