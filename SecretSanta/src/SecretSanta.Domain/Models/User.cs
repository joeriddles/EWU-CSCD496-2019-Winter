using System;
using System.Collections.Generic;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class User : Counter, IEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public List<Gift> Gifts { get; set; }
		public List<UserGroup> UserGroups { get; set; }

		public User(string firstName, string lastName)
		{
			FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			Gifts = new List<Gift>();
			UserGroups = new List<UserGroup>();
		}

		public override string ToString()
		{
			return $"{Id}: {FirstName} {LastName}";
		}
	}
}
