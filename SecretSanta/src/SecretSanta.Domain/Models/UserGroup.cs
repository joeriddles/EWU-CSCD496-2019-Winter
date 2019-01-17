using System;

namespace SecretSanta.Domain.Models
{
	public class UserGroup
	{
		public int UserId { get; set; }
		public User User { get; set; }

		public int GroupId { get; set; }
		public Group Group { get; set; }

		public UserGroup(User user, Group group)
		{
			User = user ?? throw new ArgumentNullException(nameof(user));
			Group = group ?? throw new ArgumentNullException(nameof(group));
		}

		// Default constructor needed by Entity Framework
		public UserGroup() { }
	}
}
