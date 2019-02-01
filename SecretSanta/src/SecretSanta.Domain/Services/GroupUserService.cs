using System;
using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class GroupUserService : IGroupUserService
	{
		private ApplicationDbContext DbContext { get; }

		public GroupUserService(ApplicationDbContext dbContext)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public GroupUser GetGroupUser(int groupId, int userId)
		{
			return DbContext.GroupUsers.Find(groupId, userId);
		}

		public GroupUser AddGroupUser(GroupUser groupUser)
		{
			DbContext.GroupUsers.Add(groupUser);
			DbContext.SaveChanges();
			return groupUser;
		}

		public GroupUser UpdateGroupUser(GroupUser groupUser)
		{
			DbContext.GroupUsers.Update(groupUser);
			DbContext.SaveChanges();
			return groupUser;
		}

		public void RemoveGroupUser(GroupUser groupUser)
		{
			DbContext.GroupUsers.Remove(groupUser);
			DbContext.SaveChanges();
		}

		public List<User> GetAllUsersInGroup(int groupId)
		{
			return DbContext.GroupUsers
				.Where(gu => gu.GroupId == groupId)
				.Select(gu => gu.User)
				.ToList();
		}
	}
}