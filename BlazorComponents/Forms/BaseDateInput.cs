using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public class BaseDateInput<TValue> : InputBase<TValue>
	{
		#region Parameters

		[Parameter] public string DatepickerTitle { get; set; } = string.Empty;

		#endregion

		#region Dependencies

		[Inject] public IJSRuntime Js { get; set; }

		#endregion

		#region Properties and Fields

		protected string TargetId = Guid.NewGuid().ToString();

		#endregion

		#region Methods

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Js.InvokeVoidAsync("DatePicker.initialize", TargetId, DatepickerTitle);
				CurrentValueAsString = FormatValueAsString(Value);
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		protected override string? FormatValueAsString(TValue? value)
		{
			return value switch
			{
				DateTime dateTimeValue => dateTimeValue == default ? DateTime.Today.ToString("dd/MM/yyyy") : dateTimeValue.ToString("dd/MM/yyyy"),
				_ => null
			};
		}

		protected override bool TryParseValueFromString(string value, out TValue? result, out string validationErrorMessage)
		{
			result = DateTime.TryParse(value, out var validDate) ? (TValue)(object)validDate : (TValue)(object)DateTime.Today;
			validationErrorMessage = "";
			return true;
		}

		#endregion

	}
}