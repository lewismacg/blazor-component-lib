using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class CronExpressionForm
	{
		#region Parameters

		[Parameter] public DateTime? DateToBuildFrom { get; set; }
		[Parameter] public string CronExpressionValue { get; set; }
		[Parameter] public bool SmallFields { get; set; }
		[Parameter] public EventCallback<string> CronExpressionValueChanged { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		#endregion

		#region Properties and Fields

		protected List<DayOfWeek> SelectedWeekdays { get; set; } = new();
		protected List<int> SelectedDaysOfMonth { get; set; } = new();
		protected List<Month> SelectedMonths { get; set; } = new();
		protected bool ParsingError { get; set; }

		#endregion

		#region Methods

		protected async Task GenerateWeekdays(List<DayOfWeek> selectedWeekdays)
		{
			SelectedWeekdays = selectedWeekdays;
			await InvokeCronExpression();
		}

		protected async Task GenerateDaysOfMonth(List<int> selectedDaysOfMonth)
		{
			SelectedDaysOfMonth = selectedDaysOfMonth;
			await InvokeCronExpression();
		}

		protected async Task GenerateMonths(List<Month> selectedMonths)
		{
			SelectedMonths = selectedMonths;
			await InvokeCronExpression();
		}

		protected async Task InvokeCronExpression()
		{
			ParsingError = false;

			var selectedWeekdays = SelectedWeekdays.Any() ? SelectedWeekdays.OrderBy(x => x).Select(x => ((int)x).ToString()).Aggregate((x, y) => $"{x},{y}") : "*";
			var selectedDaysOfMonth = SelectedDaysOfMonth.Any() ? SelectedDaysOfMonth.OrderBy(x => x).Select(x => x.ToString()).Aggregate((x, y) => $"{x},{y}") : "*";
			var selectedMonths = SelectedMonths.Any() ? SelectedMonths.OrderBy(x => x).Select(x => ((int)x).ToString()).Aggregate((x, y) => $"{x},{y}") : "*";

			var newCronExpression = $"0 0 {selectedDaysOfMonth} {selectedMonths} {selectedWeekdays}";

			if (CronExtensions.GetNextDateForScheduleByCronExpression(newCronExpression) != default) await CronExpressionValueChanged.InvokeAsync(newCronExpression);
			else
			{
				ParsingError = true;
				SelectedDaysOfMonth = new();
				SelectedMonths = new();
				SelectedWeekdays = new();

				await CronExpressionValueChanged.InvokeAsync(CronExtensions.DefaultCronExpression);
			}
		}

		#endregion
	}
}
