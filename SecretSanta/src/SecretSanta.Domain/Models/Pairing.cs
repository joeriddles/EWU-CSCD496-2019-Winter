using System;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Pairing : Counter, IEntity
	{
		public int RecipientId { get; set; }
		public int SantaId { get; set; }
		public int GroupId { get; set; }

		public Pairing(int recipientId, int santaId, int groupId = 0)
		{
			RecipientId = recipientId > -1 ? recipientId : throw new ArgumentException(nameof(recipientId));
			SantaId = santaId > -1 ? santaId : throw new ArgumentException(nameof(santaId));
			GroupId = groupId > -1 ? groupId : throw new ArgumentException(nameof(groupId));
		}

		public override string ToString()
		{
			return $"{Id}: Recipient: {RecipientId}, Santa: {SantaId}";
		}
	}
}
