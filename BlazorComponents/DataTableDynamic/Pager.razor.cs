﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents
{
    /// <summary>
    /// BlazorComponents Pager
    /// </summary>
    public partial class Pager
    {
        [CascadingParameter(Name = "Table")]
        public ITable Table { get; set; }

        /// <summary>
        /// Always show Pager, otherwise only show if TotalPages > 1
        /// </summary>
        [Parameter]
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// Show current page number
        /// </summary>
        [Parameter]
        public bool ShowPageNumber { get; set; }

        /// <summary>
        /// Show total item count
        /// </summary>
        [Parameter]
        public bool ShowTotalCount { get; set; }

        /// <summary>
        /// Page size options
        /// </summary>
        [Parameter]
        public List<PageSizeStruct> PageSizes { get; set; } = PageSizeStruct.DefaultPageSizes;

        /// <summary>
        /// Show Page Size Options
        /// </summary>
        [Parameter]
        public bool ShowPageSizes { get; set; }

        protected override void OnInitialized()
        {
            if (Table != null && PageSizes.All(x => x.Size != Table.GetPageSize()))
            {
                PageSizes.Add(new PageSizeStruct(Table.GetPageSize(), Table.GetPageSize().ToString()));
            }
        }

        private async Task SetPageSizeAsync(ChangeEventArgs args)
        {
            if (int.TryParse(args.Value.ToString(), out int result))
            {
                await Table.SetPageSizeAsync(result).ConfigureAwait(false);
            }
        }
    }
}
