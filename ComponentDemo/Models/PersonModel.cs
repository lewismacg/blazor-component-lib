namespace ComponentDemo.Models
{
	public class PersonModel
	{
		public virtual int Id { get; set; }
		public virtual int? ManagerId { get; set; }
		public virtual string Name { get; set; }

		public virtual string GetDescription() => $"{Name} ({Id})";
	}
}
