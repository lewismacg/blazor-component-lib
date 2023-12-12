using System.Collections.Generic;

namespace BlazorComponents
{
	public enum Colour
	{
		Blue,
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
			{Colour.Blue, "text-blue"},
			{Colour.Gold, "text-yellow"},
			{Colour.Green, "text-green"},
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
