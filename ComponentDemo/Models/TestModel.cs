using System;

namespace ComponentDemo.Models
{
	public class TestModel
	{
		public DateTime Date { get; set; }
		public DateTime? Date2 { get; set; }
		public decimal MyDecimal { get; set; }
		public decimal? MyNullableDecimal { get; set; }
		public int MyInt { get; set; }
		public int? MyNullableInt { get; set; }
	}
}
