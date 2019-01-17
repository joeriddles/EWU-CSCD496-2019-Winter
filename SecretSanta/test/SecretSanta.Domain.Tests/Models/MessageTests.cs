using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class MessageTests : BaseModelTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Message_CreateMessageWithInvalidId_ExpectException()
		{
			Message message = new Message("Hello there.", -1, -1);
		}

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
