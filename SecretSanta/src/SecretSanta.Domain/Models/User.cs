using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class User : IEntity
	{
		private static int IdCounter { get; set; }

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[NotMapped]
		public List<Gift> Gifts { get; set; }
		[NotMapped]
		public List<UserGroup> UserGroups { get; set; }

		public User(string firstName, string lastName)
		{
			IdCounter++;

			Id = IdCounter;
			FirstName = firstName ?? throw new ArgumentNullException();
			LastName = lastName ?? throw new ArgumentNullException();
			Gifts = new List<Gift>();
			UserGroups = new List<UserGroup>();
		}

		public override string ToString()
		{
			return $"{Id}: {FirstName} {LastName}";
		}

		public static void ResetCounter()
		{
			IdCounter = 0;
		}
	}
}
