using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Gift : IEntity
	{
		private static int IdCounter { get; set; }

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int UserId { get; set; }
		public int OrderOfImportance { get; set; }
		public string Url { get; set; }

		public Gift(string title, string description = "", int userId = 0, int orderOfImportance = 0,
			string url = "")
		{
			IdCounter++;

			Id = IdCounter;
			Title = title;
			Description = description;
			UserId = userId;
			OrderOfImportance = orderOfImportance;
			Url = url;
		}
	}
}
