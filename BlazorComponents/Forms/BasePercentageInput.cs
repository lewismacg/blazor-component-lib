using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Globalization;

namespace BlazorComponents
{
    public class BasePercentageInput<TValue> : InputBase<TValue>
    {
        #region Parameters

        [Parameter] public bool ShowZero { get; set; }

        #endregion

        #region Properties and Fields

        protected string InputType { get; set; } = "text";

        #endregion

        #region Methods

        protected override void OnInitialized()
        {
            if (typeof(TValue) != typeof(decimal) && typeof(TValue) != typeof(decimal?)) throw new Exception("Percentage input usage is limited to decimal type.");
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
                decimal decimalValue => decimalValue == default ? null : GetPercentageDisplayValue(decimalValue),
                _ => null
            };
        }

        protected string GetPercentageDisplayValue(decimal? value)
        {
            if (value == default) return ShowZero ? "0.00%" : null;
            return value.Value.ToString("P2", CultureInfo.GetCultureInfo("en-gb"));
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            result = GetDecimalValueFromString(value);

            validationErrorMessage = "";
            return true;
        }

        protected TValue GetDecimalValueFromString(string stringValue)
        {
            var valueAsDecimal = decimal.TryParse(stringValue, out var decimalValue) ? decimalValue : (decimal?)null;
            return valueAsDecimal == default ? default : (TValue)(object)(valueAsDecimal / 100);
        }

        protected string GetValueToDisplay()
        {
            var currentValueAsParsableString = CurrentValueAsString?.Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "") ?? "";
            var currentValue = GetDecimalValueFromString(currentValueAsParsableString) as decimal?;
            return InputType switch
            {
                "text" => GetPercentageDisplayValue(currentValue),
                "number" => decimal.TryParse(currentValueAsParsableString, CultureInfo.GetCultureInfo("en-gb"), out var parsedDecimalValue) ? parsedDecimalValue.ToString() : default
            };
        }

        #endregion

    }
}