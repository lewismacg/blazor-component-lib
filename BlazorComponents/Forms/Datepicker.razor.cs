using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class Datepicker<T>
	{
		#region Parameters

		[Parameter] public string DatepickerTitle { get; set; } = string.Empty;
		[Parameter]
		public T Value
		{
			get => _value;
			set
			{
				if (_value.Equals(value)) return;

				_value = value;
				ValueChanged.InvokeAsync(value);
			}
		}
		[Parameter] public EventCallback<T> ValueChanged { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		#endregion

		#region Dependencies

		[Inject] public IJSRuntime Js { get; set; }

		#endregion

		#region Properties and Fields

		private T _value;
		protected string TargetId = Guid.NewGuid().ToString();

		protected string? CurrentValueAsString
		{
			get => FormatValueAsString(Value);
			set
			{
				if (string.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(typeof(T)) != null) Value = default!;
				else if (TryParseValueFromString(value, out var parsedValue)) Value = parsedValue!;
			}
		}

		#endregion

		#region Methods

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Js.InvokeVoidAsync("DatePicker.initialize", TargetId, DatepickerTitle);
				CurrentValueAsString = FormatValueAsString(Value);
			}
		}

		protected string? FormatValueAsString(T? value)
		{
			return value switch
			{
				DateTime dateTimeValue => dateTimeValue == default ? DateTime.Today.ToString("dd/MM/yyyy") : dateTimeValue.ToString("dd/MM/yyyy"),
				_ => null
			};
		}

		protected bool TryParseValueFromString(string value, out T? result)
		{
			var parsedDate = DateTime.TryParse(value, out var validDate) ? validDate : DateTime.Today;
			result = (T)(object)(parsedDate < new DateTime(1900, 1, 1) ? DateTime.Today : parsedDate);

			return true;
		}

		#endregion
	}
}
