using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class MessageService
	{
		private ApplicationDbContext DbContext { get; }

		public MessageService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		//CRUD: Create, Read, Update, Delete

		public void AddMessage(Message message)
		{
			DbContext.Messages.Add(message);
			DbContext.SaveChanges();
		}

		public Message GetMessageById(int messageId)
		{
			return DbContext.Messages.Find(messageId);
		}

		public List<Message> GetAllMessages()
		{
			return DbContext.Set<Message>().ToList();
		}

		public void UpdateMessage(Message message)
		{
			DbContext.Messages.Update(message);
			DbContext.SaveChanges();
		}

		public int GetMessageCount()
		{
			return DbContext.Messages.Count();
		}
	}
}
