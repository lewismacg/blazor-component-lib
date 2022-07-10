using System.Collections.Generic;

namespace BlazorComponents
{
	public enum Colour
	{
		Red,
		Gold,
		Green,
		White,
		Black,
		Grey
	}

	public static class ColourMapper
	{
		private static Dictionary<Colour, string> _colourMap = new()
		{
			{Colour.Red, "text-primary"},
			{Colour.Gold, "text-secondary"},
			{Colour.Green, "text-success"},
			{Colour.White, "text-white"},
			{Colour.Black, "text-black"},
			{Colour.Grey, "text-muted"}
		};

		public static string GetColourClass(Colour colour)
		{
			return _colourMap[colour];
		}
	}
}
