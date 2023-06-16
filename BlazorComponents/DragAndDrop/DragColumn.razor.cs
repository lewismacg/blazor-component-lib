using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class DragColumn<TModel> where TModel : class
	{
		#region Parameters

		[CascadingParameter] DragContainer<TModel> Container { get; set; }

		/// <summary>
		/// The text to display on top of the column.
		/// </summary>
		[Parameter] public string ColumnTitle { get; set; }

		/// <summary>
		/// The category value this column is responsible for.
		/// </summary>
		[Parameter] public string CategoryValue { get; set; }

		/// <summary>
		/// Additional functions for the column header area.
		/// </summary>
		[Parameter] public RenderFragment HeaderContent { get; set; }

		/// <summary>
		/// Additional functions for the column body area above the dropzone.
		/// </summary>
		[Parameter] public RenderFragment BodyContent { get; set; }

		/// <summary>
		/// A more customisable dropzone using a renderfragment
		/// </summary>
		[Parameter] public RenderFragment CustomDropzone { get; set; }

		/// <summary>
		/// Gets styling for the header using an alert type (i.e. success, danger, info etc).
		/// </summary>
		[Parameter] public NotificationType ColumnType { get; set; }

		/// <summary>
		/// Display a count of the number of models within the column.
		/// </summary>
		[Parameter] public bool ShowCount { get; set; }

		/// <summary>
		/// The height in px allowed for any additional body content.
		/// </summary>
		[Parameter] public int? BodyContentHeight { get; set; }

		/// <summary>
		/// Show or hide the body of the column.
		/// </summary>
		[Parameter] public bool ShowBody { get; set; } = true;

		/// <summary>
		/// Additional class for the drag boxes within the dropzone.
		/// </summary>
		[Parameter] public string AdditionalDragItemClass { get; set; }

		#endregion

		#region Properties and Fields

		/// <summary>
		/// The current list of models within the columns category.
		/// </summary>
		private List<TModel> _models = new();

		/// <summary>
		/// The class given to the UI to alert the user as to whether it can or cannot be dropped here.
		/// </summary>
		private string _dropClass = string.Empty;

		/// <summary>
		/// Ensures the UI updates correctly by preventing erroneous duplicate DragEnter/DragLeave events firing.
		/// </summary>
		private int _counter = 0;

		/// <summary>
		/// A class to give the header styling based upon the ColumnType.
		/// </summary>
		private string _alertClass;

		#endregion

		#region Methods

		protected override void OnParametersSet()
		{
			_models.Clear();
			_models.AddRange(Container.Models.Where(x => Container.CategoryFunction.Invoke(x).Equals(CategoryValue)));

			_alertClass = ColumnType switch
			{
				NotificationType.Info => "info",
				NotificationType.Warning => "warning",
				NotificationType.Error => "danger",
				NotificationType.Success => "success",
				_ => ""
			};
		}

		private void HandleDragEnter()
		{
			if (Container.ActiveModel == null) return;
			if (CategoryValue.Equals(Container.CategoryFunction.Invoke(Container.ActiveModel))) return;

			_counter++;
			_dropClass = "can-drop";
		}

		private void HandleDragLeave()
		{
			if (_counter > 0) _counter--;
			_dropClass = _counter == 0 ? string.Empty : "can-drop";
		}

		private async Task HandleDrop()
		{
			if (Container.ActiveModel == null) return;

			_counter = 0;
			_dropClass = string.Empty;

			if (CategoryValue.Equals(Container.CategoryFunction.Invoke(Container.ActiveModel))) return;
			await Container.UpdateModelAsync(CategoryValue);
		}

		#endregion

	}
}
