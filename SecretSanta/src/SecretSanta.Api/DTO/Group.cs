using System;

namespace SecretSanta.Api.DTO
{
	public class Group
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public Group()
		{
		}

		public Group(Domain.Models.Group group)
		{
			if (group == null) throw new ArgumentNullException(nameof(@group));

			Id = group.Id;
			Name = group.Name;
		}

		public static Domain.Models.Group ToDomain(Group dtoGroup)
		{
			if (dtoGroup == null) throw new ArgumentNullException(nameof(dtoGroup));
			return new Domain.Models.Group()
			{
				Id = dtoGroup.Id,
				Name = dtoGroup.Name
			};
		}
	}
}