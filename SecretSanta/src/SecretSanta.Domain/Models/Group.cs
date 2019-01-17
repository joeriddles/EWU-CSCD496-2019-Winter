using System;
using System.Collections.Generic;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Group : Counter, IEntity
	{
		public string Title { get; set; }
		public List<UserGroup> UserGroups { get; set; }

		public Group(string title)
		{
			Title = title ?? throw new ArgumentNullException(nameof(title));
			UserGroups = new List<UserGroup>();
		}

		public override string ToString()
		{
			return $"{Id}: {Title}";
		}
	}
}
