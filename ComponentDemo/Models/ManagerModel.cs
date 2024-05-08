using System.Collections.Generic;

namespace ComponentDemo.Models
{
	public class ManagerModel
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual List<ManagerModel> Children { get; set; } = new();

		public virtual string GetDescription() => $"{Name} ({Id})";
	}
}
