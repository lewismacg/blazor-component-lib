using BlazorComponents.Models;
using BlazorDownloadFile;
using CsvHelper;
using CsvHelper.Configuration;
using LinqKit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class Table<TableItem> : ComponentBase, ITable<TableItem>
	{
		#region Parameters

		[Parameter] public bool ShowSearchBar { get; set; }
		[Parameter] public bool ShowFooter { get; set; }
		[Parameter] public string SearchPlaceholder { get; set; }
		[Parameter] public bool ShowChildContentAtTop { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> UnknownParameters { get; set; }
		[Parameter] public string TableClass { get; set; } = "table data-table striped table-hover table-sm";
		[Parameter] public string TableHeadClass { get; set; } = "";
		[Parameter] public string TableBodyClass { get; set; } = "";
		[Parameter] public string TableFooterClass { get; set; } = "text-black bg-grey";
		[Parameter] public Func<TableItem, string> TableRowClass { get; set; }
		[Parameter] public int PageSize { private get; set; } = DEFAULT_PAGE_SIZE;
		[Parameter] public bool ColumnReorder { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public RenderFragment HeaderChildContent { get; set; }
		[Parameter] public IQueryable<TableItem> ItemsQueryable { get; set; }
		[Parameter] public IEnumerable<TableItem> Items { get; set; }
		[Parameter] public IDataLoader<TableItem> DataLoader { get; set; }
		[Parameter] public string GlobalSearch { get; set; }
		[Parameter] public string ClassWrapperOverride { get; set; } = string.Empty;
		[Parameter] public bool AllowExport { get; set; }
		[Parameter] public bool AllowTopHorizontalScroll { get; set; }
		[Parameter] public bool ExportAtTop { get; set; }
		[Parameter] public string CsvTitleText { get; set; } = "SearchResults.csv";
		[Parameter] public EventCallback<List<string>> CustomDownloadMethodCallback { get; set; }
		[Parameter] public List<TableItem> SelectedItems { get; set; } = new();
		[Parameter] public Action<TableItem> RowClickAction { get; set; }
		/// <summary>
		/// Select Type: None, Single or Multiple
		/// </summary>
		[Parameter]
		public SelectionType SelectionType
		{
			get => _selectionType;
			set
			{
				_selectionType = value;
				if (_selectionType == SelectionType.None) SelectedItems.Clear();
				else if (_selectionType == SelectionType.Single && SelectedItems.Count > 1) SelectedItems.RemoveRange(1, SelectedItems.Count - 1);

				StateHasChanged();
			}
		}

		#endregion

		#region Dependencies

		[Inject] protected IJSRuntime JsRuntime { get; set; }
		[Inject] protected IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
		[Inject] private ILogger<ITable<TableItem>> Logger { get; set; }

		#endregion

		#region Properties and Fields

		private int _pageSize;
		private const int DEFAULT_PAGE_SIZE = 10;
		private bool VisibilityMenuOpen { get; set; }
		public IEnumerable<TableItem> FilteredItems { get; private set; }
		public List<IColumn<TableItem>> Columns { get; } = new();
		public int PageNumber { get; private set; }
		public int TotalCount { get; private set; }
		public bool IsEditMode { get; private set; }
		private bool DetailViewAvailable { get; set; }
		public int TotalPages => _pageSize <= 0 ? 1 : (TotalCount + _pageSize - 1) / _pageSize;
		private List<CustomRow<TableItem>> CustomRows { get; set; } = new();
		private string ScrollableId { get; set; } = Guid.NewGuid().ToString();
		private ElementReference ColumnMenuVisibilityRef { get; set; }
		private Dictionary<int, bool> detailsViewOpen = new();
		private IColumn<TableItem> DragSource;
		private DetailTemplate<TableItem> _detailTemplate;
		private SelectionType _selectionType;
		private RenderFragment _emptyDataTemplate;
		private RenderFragment _loadingDataTemplate;

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			_pageSize = PageSize; // Needed for persistence over StateHasChanged events from parent components, prevents reset of pagination
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && AllowTopHorizontalScroll) await JsRuntime.InvokeVoidAsync("DraggableElement.createTopTableScrollbar", ScrollableId, $"leftScrollButton-{ScrollableId}", $"rightScrollButton-{ScrollableId}");
		}

		protected override async Task OnParametersSetAsync()
		{
			await UpdateAsync().ConfigureAwait(false);
		}

		private IEnumerable<TableItem> GetData()
		{
			if (Items == null && ItemsQueryable == null)
			{
				return Items;
			}
			if (Items != null)
			{
				ItemsQueryable = Items.AsQueryable();
			}

			foreach (var item in Columns)
			{
				if (item.Filter != null)
				{
					ItemsQueryable = ItemsQueryable.Where(item.Filter);
				}
			}

			if (DataLoader != null)
			{
				return ItemsQueryable.ToList();
			}
			// Global Search
			if (!string.IsNullOrEmpty(GlobalSearch))
			{
				ItemsQueryable = ItemsQueryable.Where(GlobalSearchQuery(GlobalSearch));
			}

			TotalCount = ItemsQueryable.Count();

			var sortColumn = Columns.Find(x => x.SortColumn);

			if (sortColumn != null)
			{
				ItemsQueryable = sortColumn.SortDescending ? ItemsQueryable.OrderByDescending(sortColumn.Field) : ItemsQueryable.OrderBy(sortColumn.Field);
			}

			// if the current page is filtered out, we should go back to a page that exists
			if (PageNumber >= TotalPages)
			{
				PageNumber = TotalPages - 1;
			}

			// if PageSize is zero, we return all rows and no paging
			return _pageSize <= 0 ? ItemsQueryable.ToList() : ItemsQueryable.Skip(PageNumber * _pageSize).Take(_pageSize).ToList();
		}

		public void ToggleDetailView(int row, bool open)
		{
			if (!detailsViewOpen.ContainsKey(row)) throw new KeyNotFoundException("Specified row could not be found in the currently rendered part of the table.");

			detailsViewOpen[row] = open;
		}

		public void ToggleAllDetailsView(bool open)
		{
			int[] rows = new int[detailsViewOpen.Keys.Count];
			detailsViewOpen.Keys.CopyTo(rows, 0);
			foreach (int row in rows)
			{
				detailsViewOpen[row] = open;
			}
		}

		public async Task UpdateAsync()
		{
			await LoadServerSideDataAsync().ConfigureAwait(false);
			FilteredItems = GetData();
			Refresh();
		}

		private async Task LoadServerSideDataAsync()
		{
			if (DataLoader != null)
			{
				var sortColumn = Columns.Find(x => x.SortColumn);
				var sortExpression = new StringBuilder();
				if (sortColumn != null)
				{
					sortExpression
						.Append(sortColumn.Field.GetPropertyMemberInfo()?.Name)
						.Append(' ')
						.Append(sortColumn.SortDescending ? "desc" : "asc");
				}

				var result = await DataLoader.LoadDataAsync(new FilterData
				{
					Top = _pageSize,
					Skip = PageNumber * _pageSize,
					Query = GlobalSearch,
					OrderBy = sortExpression.ToString()
				}).ConfigureAwait(false);
				Items = result.Records;
				TotalCount = result.Total.GetValueOrDefault(1);
			}
		}

		public void AddColumn(IColumn<TableItem> column)
		{
			column.Table = this;

			if (column.Type == null)
			{
				column.Type = column.Field?.GetPropertyMemberInfo().GetMemberUnderlyingType();
			}

			Columns.Add(column);
			Refresh();
		}

		public void RemoveColumn(IColumn<TableItem> column)
		{
			Columns.Remove(column);
			Refresh();
		}

		public async Task FirstPageAsync()
		{
			if (PageNumber != 0)
			{
				PageNumber = 0;
				detailsViewOpen.Clear();
				await UpdateAsync().ConfigureAwait(false);
			}
		}

		public async Task NextPageAsync()
		{
			if (PageNumber + 1 < TotalPages)
			{
				PageNumber++;
				detailsViewOpen.Clear();
				await UpdateAsync().ConfigureAwait(false);
			}
		}

		public async Task PreviousPageAsync()
		{
			if (PageNumber > 0)
			{
				PageNumber--;
				detailsViewOpen.Clear();
				await UpdateAsync().ConfigureAwait(false);
			}
		}

		public async Task LastPageAsync()
		{
			PageNumber = TotalPages - 1;
			detailsViewOpen.Clear();
			await UpdateAsync().ConfigureAwait(false);
		}

		public void ToggleEditMode()
		{
			IsEditMode = !IsEditMode;
			StateHasChanged();
		}

		public void Refresh()
		{
			InvokeAsync(StateHasChanged);
		}

		private void HandleDragStart(IColumn<TableItem> column)
		{
			DragSource = column;
		}

		private void HandleDrop(IColumn<TableItem> column)
		{
			if (DragSource != null)
			{
				int index = Columns.FindIndex(a => a == column);

				Columns.Remove(DragSource);
				Columns.Insert(index, DragSource);
				DragSource = null;

				StateHasChanged();
			}
		}

		private string RowClass(TableItem item)
		{
			return TableRowClass?.Invoke(item);
		}

		protected void MakeColumnVisible(IColumn<TableItem> column)
		{
			column.Visible = true;
			if (Columns.All(x => x.Visible)) VisibilityMenuOpen = false;
		}

		public void SetEmptyDataTemplate(EmptyDataTemplate emptyDataTemplate)
		{
			_emptyDataTemplate = emptyDataTemplate?.ChildContent;
		}

		public void SetLoadingDataTemplate(LoadingDataTemplate loadingDataTemplate)
		{
			_loadingDataTemplate = loadingDataTemplate?.ChildContent;
		}

		public void SetDetailTemplate(DetailTemplate<TableItem> detailTemplate)
		{
			_detailTemplate = detailTemplate;
			DetailViewAvailable = Items.Any(x => detailTemplate.ShowDetailView.Compile().Invoke(x));
		}

		private void OnRowClickHandler(TableItem tableItem)
		{
			try
			{
				RowClickAction?.Invoke(tableItem);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "RowClickAction threw an exception: {0}", ex);
			}

			switch (SelectionType)
			{
				case SelectionType.None:
					return;
				case SelectionType.Single:
					SelectedItems.Clear();
					SelectedItems.Add(tableItem);
					break;
				case SelectionType.Multiple:
					if (SelectedItems.Contains(tableItem))
						SelectedItems.Remove(tableItem);
					else
						SelectedItems.Add(tableItem);
					break;
			}
		}

		public void AddCustomRow(CustomRow<TableItem> customRow)
		{
			CustomRows.Add(customRow);
		}

		private Expression<Func<TableItem, bool>> GlobalSearchQuery(string value)
		{
			Expression<Func<TableItem, bool>> expression = null;

			foreach (string keyword in value.Trim().Split(" "))
			{
				Expression<Func<TableItem, bool>> tmp = null;

				foreach (var column in Columns.Where(x => x.Field != null))
				{
					var newQuery = Expression.Lambda<Func<TableItem, bool>>(
						Expression.AndAlso(
							column.Field.Body.CreateNullChecks(),
							Expression.GreaterThanOrEqual(
								Expression.Call(
									Expression.Call(column.Field.Body, "ToString", Type.EmptyTypes),
									typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(StringComparison) }),
									new[] { Expression.Constant(keyword), Expression.Constant(StringComparison.OrdinalIgnoreCase) }),
							Expression.Constant(0))),
							column.Field.Parameters[0]);

					if (tmp == null)
						tmp = newQuery;
					else
						tmp = tmp.Or(newQuery);
				}

				if (expression == null)
					expression = tmp;
				else
					expression = expression.And(tmp);
			}

			return expression;
		}

		public async Task SetPageSizeAsync(int newPageSize)
		{
			PageNumber = (PageNumber * _pageSize) / newPageSize;
			_pageSize = newPageSize;
			await UpdateAsync().ConfigureAwait(false);
		}

		public int GetPageSize() => _pageSize;

		protected void DownloadCsv()
		{
			try
			{
				var includeColumns = Columns.Where(x => x.Visible && x.Field != null).Select(x => GetMemberName(x.Field.Body)).ToList();

				if (CustomDownloadMethodCallback.HasDelegate) CustomDownloadMethodCallback.InvokeAsync(includeColumns);
				else
				{
					var result = WriteCsvToMemory(includeColumns);
					BlazorDownloadFileService.DownloadFile(CsvTitleText, result, "text/csv");
				}
			}
			catch (Exception e)
			{
				Console.Write(e);
			}
		}

		private byte[] WriteCsvToMemory(List<string> includeColumns)
		{
			using (var memoryStream = new MemoryStream())
			using (var streamWriter = new StreamWriter(memoryStream))
			using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture))
			{
				var classmap = new DefaultClassMap<TableItem>();
				foreach (var property in from includeColumn in includeColumns let property = typeof(TableItem).GetProperty(includeColumn) select property)
				{
					classmap.Map(typeof(TableItem), property);
				}

				csvWriter.Context.RegisterClassMap(classmap);
				csvWriter.WriteRecords(ItemsQueryable);
				streamWriter.Flush();
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Returns the member name under evaluation by the expression.
		/// CAUTION: Largely untested, only tested as far as current requirements.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException"></exception>
		private static string GetMemberName(Expression expression)
		{
			return expression.NodeType switch
			{
				ExpressionType.MemberAccess => ((MemberExpression)expression).Member.Name,
				ExpressionType.Convert => GetMemberName(((UnaryExpression)expression).Operand),
				ExpressionType.Conditional => GetMemberName(((ConditionalExpression)expression).IfTrue),
				ExpressionType.Coalesce => GetMemberName(((BinaryExpression)expression).Left),
				_ => throw new NotSupportedException(expression.NodeType.ToString())
			};
		}

		#endregion
	}
}
