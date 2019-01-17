using System;
using SecretSanta.Domain.Interfaces;

namespace SecretSanta.Domain.Models
{
	public class Message : Counter, IEntity
	{
		public string Content { get; set; }
		public int RecipientId { get; set; }
		public int SantaId { get; set; }

		public Message(string content, int recipientId, int santaId)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
			RecipientId = recipientId > -1 ? recipientId : throw new ArgumentException(nameof(recipientId));
			SantaId = santaId > -1 ? santaId : throw new ArgumentException(nameof(santaId));
		}

		public override string ToString()
		{
			return $"{Id}: {Content}";
		}
	}
}
