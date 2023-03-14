namespace BlazorComponents.Tabs
{
	/// <summary>
	/// Instead of removing and re-adding the child content of the page, this loads it on first open, and then hides it using the html attribute 'hidden'.
	/// This works well in situations where you do not want to continuously reload data after the first load, but not well in situations where data is dynamic
	/// and reliant on other components to update.
	/// </summary>
	public partial class CachedTabPage
	{
		private bool ChildContentRendered { get; set; }
		public override void ActivatePage()
		{
			ChildContentRendered = true;
			base.ActivatePage();
		}
	}
}