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
			if (user is null || group is null)
				throw new ArgumentNullException();

			User = user;
			Group = group;
		}

		public UserGroup() { }
	}
}
