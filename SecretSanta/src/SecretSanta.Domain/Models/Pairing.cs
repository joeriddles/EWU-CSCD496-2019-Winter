using System;
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
			RecipientId = recipientId > -1 ? recipientId : throw new ArgumentException();
			SantaId = santaId > -1 ? santaId : throw new ArgumentException();
			GroupId = groupId > -1 ? groupId : throw new ArgumentException();
		}

		public override string ToString()
		{
			return $"{Id}: Recipient: {RecipientId}, Santa: {SantaId}";
		}

		public static void ResetCounter()
		{
			IdCounter = 0;
		}
	}
}
