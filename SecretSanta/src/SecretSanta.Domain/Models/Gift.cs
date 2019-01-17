using System;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Gift : Counter, IEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int UserId { get; set; }
		public int OrderOfImportance { get; set; }
		public string Url { get; set; }

		public Gift(string title, string description = "", int userId = 0, int orderOfImportance = 0,
			string url = "")
		{
			Title = title ?? throw new ArgumentNullException(nameof(title));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			UserId = userId > -1 ? userId : throw new ArgumentException(nameof(userId));
			OrderOfImportance = orderOfImportance > -1 ? orderOfImportance : throw new ArgumentException(nameof(orderOfImportance));
			Url = url ?? throw new ArgumentNullException(nameof(url));
		}

		public override string ToString()
		{
			return $"{Id}: {Title} {Description}";
		}
	}
}
