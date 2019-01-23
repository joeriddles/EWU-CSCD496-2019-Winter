using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Import
{
	public class Import
	{
		public string Filename { get; }

		public Import(string filename)
		{
			if (filename is null)
				throw new ArgumentNullException(nameof(filename));
			if (!File.Exists(filename))
				throw new ArgumentException(nameof(filename), "File does not exist.");
			if (!File.ReadLines(filename).Any())
				throw new ArgumentException(nameof(filename), "File does not contain header.");

			Filename = filename;
		}

		public User ReadHeader()
		{
			string header = File.ReadLines(Filename).First();
			string[] headerArray = header.Split(':');
			if (headerArray.Length != 2 || !headerArray[0].Equals("Name"))
				throw new ArgumentException(nameof(Filename), "Header formatted incorrectly.");

			string name = headerArray[1].Trim();
			string[] nameFormat = name.Split(',', 2);

			if (nameFormat.Length == 1)
			{
				string[] splitName = nameFormat[0].Split();
				return new User(splitName[0].Trim(), splitName[1].Trim()); // <FirstName> <LastName>
			}
			if (nameFormat.Length == 2)
				return new User(nameFormat[1].Trim(), nameFormat[0].Trim()); // <LastName>, <FirstName>
			throw new ArgumentException(nameof(Filename), "Header name formatted incorrectly.");
		}

		public List<Gift> ReadBody(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "Cannot add gifts before user has been created.");

			List<Gift> gifts = new List<Gift>();
			IEnumerable<string> body = File.ReadLines(Filename).Skip(1);
			foreach (var line in body.Where(line => line.Trim() != ""))
				gifts.Add(new Gift(line.Trim(), "", user.Id));

			return gifts;
		}
	}
}
