using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	interface IGroupService
	{
		Group AddGroup(Group group);
		Group UpdateGroup(Group group);
		List<Group> FetchAll();
	}
}