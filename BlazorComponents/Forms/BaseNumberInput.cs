using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorComponents
{
	public class BaseNumberInput<TValue> : InputBase<TValue>
	{
		#region Methods

		protected void BindValue(ChangeEventArgs e)
		{
			var valueAsDecimal = decimal.TryParse(e.Value?.ToString(), out var decimalValue) ? decimalValue : (decimal?)null;
			CurrentValueAsString = valueAsDecimal?.ToString();
		}

		protected override string FormatValueAsString(TValue value)
		{
			return value switch
			{
				int intValue => intValue == default ? null : intValue.ToString(),
				decimal decimalValue => decimalValue == default ? null : decimalValue.ToString(),
				_ => null
			};
		}

		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			var parsedDecimal = decimal.TryParse(value, out var decimalValue) ? decimalValue : (decimal?)null;

			if (parsedDecimal == default)
			{
				result = default;
				validationErrorMessage = "";
				return true;
			}

			if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
			{
				// Check integer has no decimal places.
				if (parsedDecimal % 1 == 0)
				{
					result = (TValue)(object)(int)parsedDecimal;

					validationErrorMessage = "";
					return true;
				}

				result = default;
				validationErrorMessage = "Decimal places are not allowed.";
				return false;
			}

			result = (TValue)(object)parsedDecimal;

			validationErrorMessage = "";
			return true;
		}

		#endregion

	}
}