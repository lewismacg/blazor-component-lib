using System.Collections.Generic;

namespace BlazorComponents
{
	public class PageSizeStruct
	{
		public int Size { get; set; }
		public string DisplayText { get; set; }

		public PageSizeStruct(int size, string text)
		{
			Size = size;
			DisplayText = text;
		}

		public static List<PageSizeStruct> DefaultPageSizes = new()
		{
			new(5, "5"),
			new(10, "10"),
			new(25, "25"),
			new(50, "50"),
			new(100, "100")
		};
	}
}
