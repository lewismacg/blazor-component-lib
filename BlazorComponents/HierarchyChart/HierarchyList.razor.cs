using BlazorComponents.HierarchyChart;
using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class HierarchyList<TObject, TKey> where TObject : class
	{
		#region Parameters

		[CascadingParameter] public HierarchyChart<TObject, TKey> HierarchyChart { get; set; }
		[Parameter] public HierarchyModel<TObject> Model { get; set; }
		[Parameter] public EventCallback<TObject> OnClickCallback { get; set; }

		#endregion
	}
}
