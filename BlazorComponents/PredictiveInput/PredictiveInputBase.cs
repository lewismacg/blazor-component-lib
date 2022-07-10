using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class PredictiveInputBase : ComponentBase
	{
		#region Parameters

		[Parameter] public IEnumerable<string> DataList { get; set; }
		[Parameter] public EventCallback<string> ValueChangedCallback { get; set; }
		[Parameter] public string Placeholder { get; set; }
		[Parameter] public int MaxSuggestions { get; set; }
		[Parameter] public bool IsSmall { get; set; }

		#endregion

		#region Properties and Fields

		public string? SelectedResult { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (DataList == null) throw new InvalidOperationException("A list of strings must be supplied in order to use predictive search");
		}

		protected async Task<IEnumerable<string>> Search(string searchText)
		{
			return await Task.FromResult(DataList.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList());
		}

		public Task OnValueChanged(string res)
		{
			return ValueChangedCallback.InvokeAsync(res);
		}

		public async Task SelectedResultChanged(string result)
		{
			SelectedResult = result;
			await OnValueChanged(result);
		}

		#endregion

	}
}
