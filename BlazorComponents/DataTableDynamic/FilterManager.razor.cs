﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorComponents
{
    public partial class FilterManager<TableItem>
    {
        [CascadingParameter(Name = "Column")]
        public IColumn<TableItem> Column { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public ILogger<FilterManager<TableItem>> Logger { get; set; }

        private async Task ApplyFilterAsync()

        {
            Column.ToggleFilter();

            if (Column.FilterControl != null)
            {
                Column.Filter = Column.FilterControl.GetFilter();
                await Column.Table.UpdateAsync().ConfigureAwait(false);
                await Column.Table.FirstPageAsync().ConfigureAwait(false);
            }
            else
            {
                Logger.LogInformation("Filter is null");
            }
        }

        private async Task ClearFilterAsync()
        {
            Column.ToggleFilter();

            if (Column.Filter != null)
            {
                Column.Filter = null;
                await Column.Table.UpdateAsync().ConfigureAwait(false);
            }
        }
    }
}
