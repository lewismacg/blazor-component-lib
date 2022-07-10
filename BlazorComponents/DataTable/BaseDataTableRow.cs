using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace BlazorComponents
{
	public class BaseDataTableRow<TTableItemType> : ComponentBase
	{
		#region Parameters

		[CascadingParameter] public BaseDataTable<TTableItemType> Table { get; set; }
		[Parameter] public TTableItemType Item { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public Action<MouseEventArgs> OnClickAction { get; set; }
		[Parameter] public Func<TTableItemType, string> RowClassFunction { get; set; }

		#endregion

		#region Properties and Fields

		public string Class { get; set; }

		#endregion

		#region Methods

		protected void OnClickHandler(MouseEventArgs e)
		{
			OnClickAction?.Invoke(e);
		}

		protected override void OnParametersSet()
		{
			if (RowClassFunction != null) Class = RowClassFunction.Invoke(Item);

		}

		#endregion
	}
}
