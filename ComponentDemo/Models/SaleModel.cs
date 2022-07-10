using System;

namespace ComponentDemo.Models
{
	public class SaleModel
	{
		public string SalesPerson { get; set; }
		public string CustomerName { get; set; }
		public string SaleName { get; set; }
		public decimal SaleValue { get; set; }
		public decimal SaleProfit { get; set; }
		public int ProductCount { get; set; }
		public DateTime? Processed { get; set; }
	}
}
