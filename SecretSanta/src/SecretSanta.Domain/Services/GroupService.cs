using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class GroupService
	{
		private ApplicationDbContext DbContext { get; }

		public GroupService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		//CRUD: Create, Read, Update, Delete

		public void AddGroup(Group group)
		{
			DbContext.Groups.Add(group);
			DbContext.SaveChanges();
		}

		public Group GetGroupById(int groupId)
		{
			return DbContext.Groups.Find(groupId);
		}

		public List<Group> GetAllGroups()
		{
			return DbContext.Set<Group>().ToList();
		}

		public void UpdateGroup(Group group)
		{
			DbContext.Groups.Update(group);
			DbContext.SaveChanges();
		}

		public int GetGroupCount()
		{
			return DbContext.Groups.Count();
		}
	}
}
