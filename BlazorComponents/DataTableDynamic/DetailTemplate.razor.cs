using BlazorComponents;
using Microsoft.AspNetCore.Components;

namespace BlazorComponents
{
    /// <summary>
    /// Detail Template
    /// </summary>
    /// <typeparam name="TableItem"></typeparam>
    public partial class DetailTemplate<TableItem> : ComponentBase
    {
        /// <summary>
        /// Parent table
        /// </summary>
        [CascadingParameter(Name = "Table")]
        public ITable<TableItem> Table { get; set; }

        /// <summary>
        /// Content to show
        /// </summary>
        [Parameter]
        public RenderFragment<TableItem> ChildContent { get; set; }

        /// <summary>
        /// When initialized, tell table of this item
        /// </summary>
        protected override void OnInitialized()
        {
            Table.SetDetailTemplate(this);
        }
    }
}
