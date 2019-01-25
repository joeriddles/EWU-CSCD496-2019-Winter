using System;

namespace SecretSanta.Api.DTO
{
	public class Gift
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int OrderOfImportance { get; set; }
		public string Url { get; set; }

		/*
			Empty constructor for serialization.
			DTO objects should always have a default constructor and all serializable properties
			should be public and have a setter
		*/	
		public Gift()
		{

		}

		// "CopyConstructor" pattern
		public Gift(Domain.Models.Gift gift)
		{
			if (gift == null) throw new ArgumentNullException(nameof(gift));

			Id = gift.Id;
			Title = gift.Title;
			Description = gift.Description;
			OrderOfImportance = gift.OrderOfImportance;
			Url = gift.Url;
		}
	}
}
