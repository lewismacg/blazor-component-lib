using CronExpressionDescriptor;
using Cronos;
using System;

namespace BlazorComponents
{
	public static class CronExtensions
	{
		public static string DefaultCronExpression => "0 0 * * *";

		public static DateTime? GetNextDateForScheduleByCronExpression(string cronExpression, DateTimeOffset? dateToBuildFrom = null)
		{
			if (cronExpression == null) throw new ArgumentNullException(nameof(cronExpression));

			var cronExpressionArray = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (cronExpressionArray.Length > 5) throw new Exception("Only 5 parts are allowed for this extension method.");

			var nextDateForSchedule = CronExpression.Parse(cronExpression).GetNextOccurrence(dateToBuildFrom ?? DateTimeOffset.Now, TimeZoneInfo.Local);

			return nextDateForSchedule?.DateTime;
		}

		public static string UserFriendlyCronExpression(string cronExpression, bool ignoreTime = false)
		{
			var generatedDescription = ExpressionDescriptor.GetDescription(cronExpression, new Options
			{
				Use24HourTimeFormat = true
			});

			if (ignoreTime)
			{
				var timeSplit = generatedDescription.Split(',', 2);
				if (timeSplit.Length == 1) return "Everyday";

				// Ensures the first letter is capitalised.
				var cronDescription = timeSplit[1].Trim();
				return cronDescription[0].ToString().ToUpper() + cronDescription[1..];
			}

			return generatedDescription;
		}
	}
}
