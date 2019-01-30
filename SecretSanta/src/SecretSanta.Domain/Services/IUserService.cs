using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public interface IUserService
	{
		User GetUser(int userId);
		User AddUser(User user);
		User UpdateUser(User user);
		List<User> FetchAll();
		void RemoveUser(User user);
	}
}