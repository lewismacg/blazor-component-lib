using Microsoft.AspNetCore.Components.Forms;

namespace BlazorComponents
{
	public class BaseNumberInput<TValue> : InputBase<TValue>
	{
		/// <summary>
		/// If the user manually inputs a zero we need to ensure it stays in the input.
		/// Without this boolean a 'zero' results in an empty looking input for non-nullable integers.
		/// </summary>
		private bool _zeroEntered { get; set; }

		#region Methods

		protected override string FormatValueAsString(TValue value)
		{
			var valueType = typeof(TValue);
			if (valueType == typeof(int?) || valueType == typeof(decimal?)) return value?.ToString();

			if (_zeroEntered) return "0";

			if (valueType == typeof(decimal))
			{
				var decimalValue = (decimal)(object)value;
				return decimalValue == default ? null : decimalValue.ToString();
			}

			var intValue = (int)(object)value;
			return intValue == default ? null : intValue.ToString();
		}

		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			var parsedDecimal = decimal.TryParse(value, out var decimalValue) ? decimalValue : (decimal?)null;

			_zeroEntered = parsedDecimal == 0;

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