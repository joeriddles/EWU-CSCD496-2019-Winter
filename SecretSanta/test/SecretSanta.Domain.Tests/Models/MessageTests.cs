using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class MessageTests
	{
		[TestMethod]
		public void CreateMessage()
		{
			Message message = new Message(content: "Who goes there?", recipientId: 1, santaId: 2);
			Assert.AreEqual("Who goes there?", message.Content);
			Assert.AreEqual(1, message.RecipientId);
			Assert.AreEqual(2, message.SantaId);
		}
	}
}
