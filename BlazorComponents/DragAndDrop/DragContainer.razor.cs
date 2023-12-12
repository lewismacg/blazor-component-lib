using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
	public partial class DragContainer<TModel> : IAsyncDisposable where TModel : class
	{
		#region Parameters

		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// A list of models to be used within the component, split into columns via the CategoryFunction
		/// </summary>
		[Parameter] public List<TModel> Models { get; set; }

		/// <summary>
		/// Callback function upon updating the column for a model.
		/// </summary>
		[Parameter] public EventCallback<TModel> OnCategoryUpdated { get; set; }

		/// <summary>
		/// A setter function for setting a property value on a model upon category update.
		/// </summary>
		[Parameter] public Func<TModel, object, bool> OnDrop { get; set; }

		/// <summary>
		/// A get function for getting the text to be displayed for a model.
		/// </summary>
		[Parameter] public Func<TModel, string> TextFunction { get; set; }

		/// <summary>
		/// A get function for getting the models current category to discern it's appropriate column.
		/// </summary>
		[Parameter] public Func<TModel, object> CategoryFunction { get; set; }

		/// <summary>
		/// Any additional classes for the drop container.
		/// </summary>
		[Parameter] public string AdditionalClass { get; set; }

		#endregion

		#region Dependencies

		[Inject] public IJSRuntime JsRuntime { get; set; }

		#endregion

		#region Properties and Fields

		/// <summary>
		/// The currently active model for the container (i.e. the one that's being dragged).
		/// </summary>
		public TModel ActiveModel { get; set; }

		#endregion

		#region Methods

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JsRuntime.InvokeVoidAsync("DraggableElement.increaseDragScrollBoundary");
			}
		}

		public async Task UpdateModelAsync(object newCategoryValue)
		{
			var model = Models.SingleOrDefault(x => x == ActiveModel);

			if (model != null)
			{
				OnDrop.Invoke(model, newCategoryValue);
				await OnCategoryUpdated.InvokeAsync(ActiveModel);
			}
		}

		#endregion

		public async ValueTask DisposeAsync()
		{
			await JsRuntime.InvokeVoidAsync("DraggableElement.destroyDragScrollBoundary");
		}
	}
}
