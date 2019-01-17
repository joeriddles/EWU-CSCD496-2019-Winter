using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class UserService
	{
		private ApplicationDbContext DbContext { get; }

		public UserService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		//CRUD: Create, Read, Update, Delete

		public void AddUser(User user)
		{
			DbContext.Users.Add(user);
			DbContext.SaveChanges();
		}

		public User GetUserById(int userId)
		{
			return DbContext.Users.Find(userId);
		}

		public List<User> GetAllUsers()
		{
			return DbContext.Set<User>().ToList();
		}

		public void UpdateUser(User user)
		{
			DbContext.Users.Update(user);
			DbContext.SaveChanges();
		}

		public User DeleteUser(int userId)
		{
			User user = DbContext.Users.Find(userId);

			if (user is null)
				return null;

			DbContext.Users.Remove(user);
			DbContext.SaveChanges();
			return user;
		}

		public void DeleteAllUsers()
		{
			List<User> allUsers = GetAllUsers();
			allUsers.ForEach(user => DeleteUser(user.Id));
			DbContext.SaveChanges();
		}

		public int GetUserCount()
		{
			return DbContext.Users.Count();
		}
	}
}
