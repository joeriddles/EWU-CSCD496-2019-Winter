using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Message : IEntity
	{
		private static int IdCounter { get; set; }

		public int Id { get; set; }
		public string Content { get; set; }
		public int RecipientId { get; set; }
		public int SantaId { get; set; }

		public Message(string content, int recipientId, int santaId)
		{
			IdCounter++;

			Id = IdCounter;
			Content = content;
			RecipientId = recipientId;
			SantaId = santaId;
		}
	}
}
