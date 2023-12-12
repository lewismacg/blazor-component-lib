using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace BlazorComponents
{
    /// <summary>
    /// A sticky banner at the bottom of the page. Parameters allow for this banner to be dragged, hidden/shown at depth, and dismissable
    /// If using more than one banner on a page, or ideally even if using one, this component should be wrapped within the BannerWrapper.
    /// </summary>
    public partial class Banner
    {
        #region Parameters

        /// <summary>
        /// The content within the banner, i.e. your message.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The styling of the banner, warning, error, success etc.
        /// </summary>
        [Parameter] public NotificationType NotificationType { get; set; } = NotificationType.None;

        /// <summary>
        /// Allows the banner to be hidden by showing a close symbol which when clicked hides the banner.
        /// </summary>
        [Parameter] public bool AllowDismiss { get; set; } = true;

        /// <summary>
        /// Forces the banner to be smaller in size.
        /// </summary>
        [Parameter] public bool IsSmall { get; set; }

        /// <summary>
        /// Shows a drag icon that allows the user to move the banner around the screen.
        /// </summary>
        [Parameter] public bool IsDraggable { get; set; }

        /// <summary>
        /// The depth at which the banner should be shown. If empty, the banner is shown at all depths.
        /// </summary>
        [Parameter] public int? DepthForDisplay { get; set; }

        /// <summary>
        /// Optional ID for the banner.
        /// </summary>
        [Parameter] public string BannerId { get; set; } = Guid.NewGuid().ToString("N");

        #endregion

        #region Dependencies

        [Inject] private IJSRuntime Runtime { get; set; }

        #endregion

        #region Properties and Fields

        public string NotificationBannerClass { get; set; }
        public Colour IconColour { get; set; }
        public string IconName { get; set; }
        protected string BannerHandleId => $"{BannerId}Handle";

        #endregion

        #region Methods

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                NotificationBannerClass = NotificationType.ToString();
                IconColour = DiscernIconColour(NotificationType);
                IconName = DiscernIconName(NotificationType);

                if (IsDraggable) Runtime.InvokeVoidAsync("DraggableElement.makeDraggable", BannerId, BannerHandleId);
                if (DepthForDisplay.HasValue) Runtime.InvokeVoidAsync("ScrollElement.showOnScroll", BannerId, "translucent", DepthForDisplay);
                StateHasChanged();
            }

            base.OnAfterRender(firstRender);
        }

        public void HideBanner()
        {
            NotificationBannerClass += " banner-hide";
            StateHasChanged();
        }

        private Colour DiscernIconColour(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Blue => Colour.Black,
                NotificationType.Yellow => Colour.Black,
                NotificationType.Red => Colour.Black,
                NotificationType.Green => Colour.White,
                NotificationType.None => Colour.Grey,
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
            };
        }

        private string DiscernIconName(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Blue => "notifications_none",
                NotificationType.Yellow => "help_outline",
                NotificationType.Red => "info_outline",
                NotificationType.Green => "check_circle_outline",
                NotificationType.None => "lightbulb",
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
            };
        }

        #endregion

    }
}
