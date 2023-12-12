using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class DragCustomArea<TModel> where TModel : class
	{
		#region Parameters

		[CascadingParameter] DragContainer<TModel> Container { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public object CategoryValue { get; set; }
		[Parameter] public string DropzoneClass { get; set; } = "bold-border";

		#endregion

		#region Properties and Fields

		private string _dropClass = string.Empty;
		private int _counter;

		#endregion

		#region Methods

		private void HandleDragEnter()
		{
			if (Container.ActiveModel == null) return;

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

			if (Container.CategoryFunction.Invoke(Container.ActiveModel) == CategoryValue) return;
			await Container.UpdateModelAsync(CategoryValue);
		}

		#endregion

	}
}
