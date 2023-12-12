using Microsoft.AspNetCore.Components;
using System;

namespace BlazorComponents
{
	public partial class SplitInput
	{
		#region Parameters

		[Parameter] public RenderFragment LeftInput { get; set; }
		[Parameter] public RenderFragment RightInput { get; set; }
		[Parameter] public int? LeftWidthPercentage { get; set; }

		#endregion

		#region Properties and Fields

		private string _calcLeftWidthPercentage { get; set; }
		private string _calcRightWidthPercentage { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			if (LeftWidthPercentage.HasValue)
			{
				if (LeftWidthPercentage > 100) throw new Exception("Cannot have values greater than 100 for percentages.");

				_calcLeftWidthPercentage = $"{LeftWidthPercentage}%";
				_calcRightWidthPercentage = $"{100 - LeftWidthPercentage.Value}%";
			}
			else
			{
				_calcLeftWidthPercentage = "50%";
				_calcRightWidthPercentage = "50%";
			}
		}

		#endregion
	}
}
