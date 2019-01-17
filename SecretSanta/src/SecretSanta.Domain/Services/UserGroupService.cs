using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class UserGroupService
	{
		private ApplicationDbContext DbContext { get; }

		public UserGroupService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		public void AddUserGroup(UserGroup userGroup)
		{
			DbContext.UserGroups.Add(userGroup);
			DbContext.SaveChanges();
		}

		public UserGroup GetUserGroupByIds(int userId, int groupId)
		{
			return DbContext.UserGroups.Find(userId, groupId);
		}

		public List<UserGroup> GetAllUserGroups()
		{
			return DbContext.Set<UserGroup>().ToList();
		}

		public void UpdateUserGroup(UserGroup userGroup)
		{
			DbContext.UserGroups.Update(userGroup);
			DbContext.SaveChanges();
		}

		public UserGroup DeleteUserGroup(int userId, int groupId)
		{
			UserGroup userGroup = DbContext.UserGroups.Find(userId, groupId);

			if (userGroup is null)
				return null;

			DbContext.UserGroups.Remove(userGroup);
			DbContext.SaveChanges();
			return userGroup;
		}

		public void DeleteAllUserGroups()
		{
			List<UserGroup> allUserGroups = GetAllUserGroups();
			allUserGroups.ForEach(userGroup => DeleteUserGroup(userGroup.UserId, userGroup.GroupId));
			DbContext.SaveChanges();
		}

		public int GetUserGroupCount()
		{
			return DbContext.UserGroups.Count();
		}
	}
}
