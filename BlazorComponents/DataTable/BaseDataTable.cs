using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlazorComponents
{
	public class BaseDataTable<TTableItemType> : ComponentBase
	{
		#region Parameters

		[Parameter] public List<TTableItemType> Items { get; set; }
		[Parameter] public bool ShowPaging { get; set; }
		[Parameter] public int PageSize { get; set; }
		[Parameter] public bool ShowFilter { get; set; }
		[Parameter] public bool ShowPageSizeSelect { get; set; } = true;
		[Parameter] public bool SlimRows { get; set; }
		[Parameter] public string FieldToFilter { get; set; }
		[Parameter] public Action<MouseEventArgs, TTableItemType> OnRowClick { get; set; }
		[Parameter] public Func<TTableItemType, string> RowClassFunction { get; set; }
		[Parameter] public string ClassWrapperOverride { get; set; } = string.Empty;
		[Parameter] public RenderFragment DataTableHeader { get; set; }
		[Parameter] public RenderFragment<TTableItemType> DataTableRow { get; set; }
		[Parameter] public bool Striped { get; set; }

		#endregion

		#region Properties and Fields

		public List<TTableItemType> FilteredList => GetFilteredItemList();
		public List<TTableItemType> ItemList => GetPagedItemList();
		public string ItemFilter { get; set; }
		private string _lastItemFilter { get; set; }
		public int CurrentPage { get; set; } = 1;
		public List<PageSizeStruct> PageSizes { get; set; } = PageSizeStruct.DefaultPageSizes;
		protected int _pageSize { get; set; }

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			_pageSize = PageSize; // Needed for persistence over StateHasChanged events from parent components, prevents reset of pagination
		}

		public void NextPage()
		{
			if (_pageSize <= -1) _pageSize = FilteredList.Count;
			if (CurrentPage * _pageSize >= FilteredList.Count) return;

			CurrentPage++;
		}

		public void PreviousPage()
		{
			if (CurrentPage == 1) return;

			CurrentPage--;
		}

		public void GotoFirstPage()
		{
			CurrentPage = 1;
		}

		public void GotoLastPage()
		{
			CurrentPage = FilteredList.Count / _pageSize;

			if (FilteredList.Count > CurrentPage * _pageSize) CurrentPage++;
		}

		public string GetPageItemCountString()
		{
			var start = ((CurrentPage - 1) * _pageSize);
			if (_pageSize < 0)
			{
				start = 0;
				_pageSize = FilteredList.Count;
			}

			var end = start + _pageSize;
			if (end > FilteredList.Count) end = FilteredList.Count;

			return $"{start + 1}-{end} of {FilteredList.Count}";
		}

		public List<TTableItemType> GetPagedItemList()
		{
			var filtered = GetFilteredItemList();
			var result = new List<TTableItemType>();

			if (ShowPaging)
			{
				result.AddRange(filtered.Skip((CurrentPage - 1) * _pageSize).Take(_pageSize));
			}
			else
			{
				result.AddRange(filtered);
			}

			return result;
		}

		public List<TTableItemType> GetFilteredItemList()
		{
			if (!ShowFilter || string.IsNullOrEmpty(FieldToFilter)) return Items;
			if (string.IsNullOrEmpty(ItemFilter)) return Items;

			if (_lastItemFilter != ItemFilter) GotoFirstPage();

			var result = new List<TTableItemType>();

			var propertyInfo =
				typeof(TTableItemType).GetProperty(FieldToFilter, BindingFlags.Public | BindingFlags.Instance);

			if (propertyInfo == null)
			{
				return Items;
			}

			result.AddRange(Items.Where(it => propertyInfo.GetValue(it, null)?.ToString().ToLower().Contains(ItemFilter.ToLower()) ?? false));

			_lastItemFilter = ItemFilter;
			return result;
		}

		public void OnPageSizeChange(ChangeEventArgs args)
		{
			var newPageSize = int.Parse(args.Value.ToString());

			if (CurrentPage * newPageSize > FilteredList.Count)
			{
				CurrentPage = 1;
			}

			_pageSize = newPageSize;
			StateHasChanged();
		}

		public void OnFilterTermChange(ChangeEventArgs args)
		{
			ItemFilter = args.Value?.ToString();
		}

		#endregion
	}
}
