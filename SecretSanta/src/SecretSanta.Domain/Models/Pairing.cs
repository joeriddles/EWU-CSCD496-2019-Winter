using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Pairing : IEntity
	{
		private static int IdCounter { get; set; }

		public int Id { get; set; }
		public int RecipientId { get; set; }
		public int SantaId { get; set; }
		public int GroupId { get; set; }

		public Pairing(int recipientId, int santaId, int groupId = 0)
		{
			IdCounter++;

			Id = IdCounter;
			RecipientId = recipientId;
			SantaId = santaId;
			GroupId = groupId;
		}
	}
}
