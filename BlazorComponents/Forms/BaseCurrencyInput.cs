using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Globalization;

namespace BlazorComponents
{
    public class BaseCurrencyInput<TValue> : InputBase<TValue>
    {
        #region Parameter

        [Parameter] public bool ShowZero { get; set; } = true;

        #endregion

        #region Properties and Fields

        protected string InputType { get; set; } = "text";

        #endregion

        protected override void OnInitialized()
        {
            if (typeof(TValue) != typeof(decimal) && typeof(TValue) != typeof(decimal?)) throw new Exception("Currency input usage is limited to decimal type.");
        }

        protected void BindValue(ChangeEventArgs e)
        {
            var valueAsDecimal = decimal.TryParse(e.Value?.ToString(), out var decimalValue) ? decimalValue : (decimal?)null;
            CurrentValueAsString = valueAsDecimal == default ? null : Math.Round(valueAsDecimal.Value, 2).ToString();
        }

        protected override string FormatValueAsString(TValue value)
        {
            return value switch
            {
                decimal decimalValue => decimalValue == default ? null : decimalValue.ToString(),
                _ => null
            };
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            var valueAsDecimal = decimal.TryParse(value, out var decimalValue) ? decimalValue : (decimal?)null;
            result = valueAsDecimal == default ? default : (TValue)(object)valueAsDecimal;

            validationErrorMessage = "";
            return true;
        }

        protected void ToggleType(string type)
        {
            InputType = type;
        }

        protected string GetValueToDisplay()
        {
            return InputType switch
            {
                "text" => CurrentValueAsString == default ? ShowZero ? "£0.00" : null : decimal.Parse(CurrentValueAsString).ToString("C", CultureInfo.GetCultureInfo("en-gb")),
                "number" => decimal.TryParse(CurrentValueAsString, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-gb"), out var parsedDecimalValue) ? parsedDecimalValue.ToString() : default,
                _ => null
            };
        }
    }
}
