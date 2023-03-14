using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
	public partial class DragBox<TModel> where TModel : class
	{
		#region Parameters

		[CascadingParameter] public DragContainer<TModel> Container { get; set; }
		[Parameter] public TModel Model { get; set; }
		[Parameter] public string AdditionalClass { get; set; }

		#endregion

		#region Properties and Fields

		/// <summary>
		/// The text to display within the box.
		/// </summary>
		private string _text;

		/// <summary>
		/// A class used to style the box when being dragged.
		/// </summary>
		private string _dragClass;

		#endregion

		#region Methods

		protected override void OnParametersSet()
		{
			_text = Container.TextFunction.Invoke(Model);
		}

		protected void OnDragStart()
		{
			Container.ActiveModel = Model;
			_dragClass = "dragged";
		}

		protected void OnDragEnd()
		{
			_dragClass = string.Empty;
			Container.ActiveModel = null;
		}

		#endregion

	}
}
