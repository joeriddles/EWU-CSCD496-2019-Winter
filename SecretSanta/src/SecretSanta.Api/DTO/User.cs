using System;

namespace SecretSanta.Api.DTO
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public User()
		{
		}

		public User(Domain.Models.User user)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));

			Id = user.Id;
			FirstName = user.FirstName;
			LastName = user.LastName;
		}

		public static Domain.Models.User ToDomain(User dtoUser)
		{
			if (dtoUser == null) throw new ArgumentNullException(nameof(dtoUser));
			return new Domain.Models.User()
			{
				Id = dtoUser.Id,
				FirstName = dtoUser.FirstName,
				LastName = dtoUser.LastName
			};
		}
	}
}