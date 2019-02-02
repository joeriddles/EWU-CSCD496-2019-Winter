using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public interface IGroupService
	{
		Group GetGroup(int groupId);
		Group AddGroup(Group group);
		Group UpdateGroup(Group group);
		void RemoveGroup(Group group);
		List<Group> GetAllGroups();
	}
}