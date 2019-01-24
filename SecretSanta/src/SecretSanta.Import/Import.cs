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
				throw new ArgumentException("File does not exist.", nameof(filename));
			if (!File.ReadLines(filename).Any())
				throw new ArgumentException("File does not contain header.", nameof(filename));

			Filename = filename;
		}

		public User ReadHeader()
		{
			string header = File.ReadLines(Filename).First();
			string[] headerArray = header.Split(':');
			if (headerArray.Length != 2 || !headerArray[0].Equals("Name"))
				throw new ArgumentException("Header formatted incorrectly.", nameof(Filename));

			string name = headerArray[1].Trim();
			string[] nameFormat = name.Split(',', 2);

			if (nameFormat.Length == 1)
			{
				string[] splitName = nameFormat[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
				return new User(splitName[0].Trim(), splitName[1].Trim()); // <FirstName> <LastName>
			}
			if (nameFormat.Length == 2)
				return new User(nameFormat[1].Trim(), nameFormat[0].Trim()); // <LastName>, <FirstName>
			throw new ArgumentException("Header name formatted incorrectly.", nameof(Filename));
		}

		public List<Gift> ReadBody(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "Cannot add gifts before user has been created.");

			return File.ReadLines(Filename)
				.Skip(1)
				.Select(line => line.Trim())
				.Where(line => !line.Equals(""))
				.Select(line => new Gift(line, "", user.Id))
				.ToList();
		}
	}
}
