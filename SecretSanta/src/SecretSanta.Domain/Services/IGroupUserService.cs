using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public interface IGroupUserService
	{
		GroupUser GetGroupUser(int groupId, int userId);
		GroupUser AddGroupUser(GroupUser groupUser);
		GroupUser UpdateGroupUser(GroupUser groupUser);
		void RemoveGroupUser(GroupUser groupUser);
		List<User> GetAllUsersInGroup(int groupId);
	}
}
