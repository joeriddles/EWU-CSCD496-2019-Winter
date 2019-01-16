using System;
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
			Title = title ?? throw new ArgumentNullException();
			Description = description ?? throw new ArgumentNullException();
			UserId = userId > 0 ? userId : throw new ArgumentException();
			OrderOfImportance = orderOfImportance > 0 ? orderOfImportance : throw new ArgumentException();
			Url = url ?? throw new ArgumentNullException();
		}

		public override string ToString()
		{
			return $"{Id}: {Title} {Description}";
		}

		public static void ResetCounter()
		{
			IdCounter = 0;
		}
	}
}
