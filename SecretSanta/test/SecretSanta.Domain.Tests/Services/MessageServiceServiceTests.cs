using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class MessageServiceServiceTests : BaseServiceTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MessageService_AddNullMessage_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				messageService.AddMessage(null);
			}
		}

		[TestMethod]
		public void MessageService_GetMessageThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				Message message = messageService.GetMessageById(-1);

				Assert.IsNull(message);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MessageService_UpdateMessageWithNull_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				messageService.UpdateMessage(null);
			}
		}

		[TestMethod]
		public void MessageService_AddMessage_Success()
		{
			Message message = new Message("Hello, there", 1, 2);
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				int oldCount = messageService.GetMessageCount();
				messageService.AddMessage(message);

				Assert.AreEqual(messageService.GetMessageCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void MessageService_GetMessage_Success()
		{
			Message message = new Message("Hello, there", 1, 2);
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				messageService.AddMessage(message);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				Message fetchedMessage = messageService.GetMessageById(message.Id);

				Assert.AreEqual(message.Id, fetchedMessage.Id);
				Assert.AreEqual(message.Content, fetchedMessage.Content);
			}
		}

		[TestMethod]
		public void MessageService_UpdateMessage_Success()
		{
			Message message = new Message("Hello, there", 1, 2);
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				messageService.AddMessage(message);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				message.Content = "General Kenobi.";
				messageService.UpdateMessage(message);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				Message updatedMessage = messageService.GetMessageById(message.Id);

				Assert.AreEqual("General Kenobi.", updatedMessage.Content);
			}
		}

		[TestMethod]
		public void MessageService_GetAllMessages_Success()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				messageService.AddMessage(new Message("Hello, there", 1, 2));
				messageService.AddMessage(new Message("General Kenobi", 2, 1));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				MessageService messageService = new MessageService(context);
				List<Message> allMessages = messageService.GetAllMessages();

				Assert.AreEqual(2, allMessages.Count);
				Assert.AreEqual("Hello, there", allMessages[0].Content);
				Assert.AreEqual("General Kenobi", allMessages[1].Content);
			}
		}

		[TestInitialize]
		public void Initialize()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(new User("General", "Kenobi"));
				userService.AddUser(new User("General", "Grevious"));
			}
		}
	}
}
