using BlazorComponents.HierarchyChart;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorComponents
{
	public partial class HierarchyChart<TObject, TKey> where TObject : class
	{
		#region Parameters

		[Parameter, EditorRequired] public List<TObject> Models { get; set; }
		[Parameter, EditorRequired] public Func<TObject, TKey> KeyFunc { get; set; }
		[Parameter, EditorRequired] public Func<TObject, string> DisplayTextFunc { get; set; }
		[Parameter, EditorRequired] public HierarchyRelationshipEnum ChartDirection { get; set; }
		[Parameter] public Func<TObject, TKey> ParentKeyFunc { get; set; }
		[Parameter] public Func<TObject, List<TObject>> ChildSelectFunc { get; set; }
		[Parameter] public bool AllowFiltering { get; set; }
		[Parameter] public EventCallback<TObject> OnSelectHierarchy { get; set; }

		#endregion

		#region Properties and Fields

		protected List<HierarchyModel<TObject>> HierarchyTree { get; set; } = new();
		protected List<HierarchyModel<TObject>> FilteredHierarchyTree { get; set; } = new();
		protected string FilterText { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			if (ChartDirection == HierarchyRelationshipEnum.ChildToParent && ParentKeyFunc == null) throw new Exception("A parent selector function is required when charting a child to parent relationship.");
			if (ChartDirection == HierarchyRelationshipEnum.ParentToChildren && ChildSelectFunc == null) throw new Exception("A child selector function is required when charting a parent to child relationship.");
		}

		protected override void OnParametersSet()
		{
			HierarchyTree = ChartDirection == HierarchyRelationshipEnum.ChildToParent ? BuildTreeFromChildToParentRelationship(Models.Select(x => new HierarchyModel<TObject>(x, [])).ToList())
																					  : Models.Select(x => BuildTreeFromParentToChildRelationship(x, default)).ToList();

			FilterModels();
		}

		protected void FilterModels()
		{
			if (ChartDirection == HierarchyRelationshipEnum.ChildToParent) FilterChildToParent();
			else FilterParentToChildren();
		}

		#region ChildToParent

		private List<HierarchyModel<TObject>> BuildTreeFromChildToParentRelationship(List<HierarchyModel<TObject>> models)
		{
			var modelKeyMap = models.ToDictionary(x => KeyFunc(x.Data));
			var topHierarchyModels = new List<HierarchyModel<TObject>>();

			foreach (var model in models)
			{
				if (modelKeyMap.TryGetValueAllowNull(ParentKeyFunc(model.Data), out var parent)) parent.Children.Add(model);
				else topHierarchyModels.Add(model);
			}

			return topHierarchyModels;
		}

		private void FilterChildToParent()
		{
			FilteredHierarchyTree = [.. HierarchyTree];
			if (string.IsNullOrWhiteSpace(FilterText)) return;

			var modelsMatchingFilter = Models.Where(x => DisplayTextFunc(x).Contains(FilterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
			var modelsParentIds = modelsMatchingFilter.Select(ParentKeyFunc).ToList();

			while (modelsParentIds.Any())
			{
				var modelsMatchingParentId = Models.Where(x => modelsParentIds.Contains(KeyFunc(x))).ToList();

				modelsParentIds.Clear();
				modelsMatchingFilter.AddRange(modelsMatchingParentId);

				modelsParentIds = modelsMatchingParentId.Select(ParentKeyFunc).ToList();
			}

			FilteredHierarchyTree = BuildTreeFromChildToParentRelationship(modelsMatchingFilter.DistinctBy(KeyFunc).Select(x => new HierarchyModel<TObject>(x, [])).ToList());
		}

		#endregion

		#region ParentToChildren

		private HierarchyModel<TObject> BuildTreeFromParentToChildRelationship(TObject model, HierarchyModel<TObject> parent)
		{
			var newHierarchyModel = new HierarchyModel<TObject>(model, []);

			var children = ChildSelectFunc(model);
			if (children.Any() == false) return newHierarchyModel;

			newHierarchyModel.Children = children.Select(x => BuildTreeFromParentToChildRelationship(x, newHierarchyModel)).ToList();
			return newHierarchyModel;
		}

		private void FilterParentToChildren()
		{
			FilteredHierarchyTree = HierarchyTree.Select(x => x.Clone()).ToList();
			if (string.IsNullOrWhiteSpace(FilterText)) return;

			FilteredHierarchyTree = FilteredHierarchyTree.Where(ModelMatchesFilterOrHasChildMatchingFilter).ToList();
		}

		private bool ModelMatchesFilterOrHasChildMatchingFilter(HierarchyModel<TObject> model)
		{
			var modelsToRemove = model.Children.Where(x => ModelMatchesFilterOrHasChildMatchingFilter(x) == false).ToList();
			modelsToRemove.ForEach(x => model.Children.Remove(x));

			// If this model still has children after the recursive removal has completed for the current model, then we can assert that this model
			// should not be removed as one or more child must match the filter.
			if (model.Children.Any()) return true;

			// If this model has no children it's the current bottom of the hierarchy. If it matches the filter text then it should be kept,
			// otherwise it should be removed.
			return DisplayTextFunc(model.Data).Contains(FilterText, StringComparison.InvariantCultureIgnoreCase);
		}

		#endregion

		#endregion
	}
}
