using System.Collections.Generic;
using System.Linq;

namespace BlazorComponents.HierarchyChart
{
	public struct HierarchyModel<TModel>
	{
		public HierarchyModel(TModel data, List<HierarchyModel<TModel>> children)
		{
			Data = data;
			Children = children;
		}

		public TModel Data { get; set; }
		public List<HierarchyModel<TModel>> Children { get; set; }

		/// <summary>
		/// Performs a deep copy of the model. Required for filtering purposes, where removing a child
		/// would incorrectly remove it from both the filtered list and original list.
		/// </summary>
		/// <returns></returns>
		public HierarchyModel<TModel> Clone() => new(Data, Children.Select(x => x.Clone()).ToList());
	}
}
