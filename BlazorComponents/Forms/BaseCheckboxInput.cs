using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazorComponents
{
	public class BaseCheckboxInput : InputBase<bool>
	{
		[Parameter] public Colour IconColour { get; set; } = Colour.Black;

		protected string GetCheckboxIcon() => CurrentValue ? "check_box" : "check_box_outline_blank";
		protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
	}
}
