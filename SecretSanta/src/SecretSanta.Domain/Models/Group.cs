using System.Collections.Generic;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Group : IEntity
	{
		private static int IdCounter { get; set; }

		public int Id { get; set; }
		public string Title { get; set; }
		public List<UserGroup> UserGroups { get; set; }

		public Group(string title)
		{
			IdCounter++;

			Id = IdCounter;
			Title = title;
			UserGroups = new List<UserGroup>();
		}

		public override string ToString()
		{
			return $"{Id}: {Title}";
		}

		public static void ResetCounter()
		{
			IdCounter = 0;
		}
	}
}
