using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class NotificationSegment
	{
		#region Parameters

		[Parameter] public string Title { get; set; }
		[Parameter] public string Text { get; set; }
		[Parameter] public string FillClass { get; set; }
		[Parameter] public bool IsUnread { get; set; }
		[Parameter] public string Footer { get; set; }

		#endregion

		#region Properties and Fields

		public bool Acknowledged { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			Acknowledged = !IsUnread;
		}

		#endregion
	}
}