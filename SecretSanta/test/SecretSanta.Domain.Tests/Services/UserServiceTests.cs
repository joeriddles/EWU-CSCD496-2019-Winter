using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class UserServiceTests : BaseTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UserService_AddNullUser_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(null);
			}
		}

		[TestMethod]
		public void UserService_GetUserThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				User user = userService.GetUserById(-1);

				Assert.IsNull(user);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UserService_UpdateUserWithNull_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.UpdateUser(null);
			}
		}

		[TestMethod]
		public void UserService_AddUser_Success()
		{
			User user = new User("Inigo", "Montoya");

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				int oldCount = userService.GetUserCount();
				userService.AddUser(user);

				Assert.AreEqual(userService.GetUserCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void UserService_GetUser_Success()
		{
			User user = new User("Inigo", "Montoya");

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(user);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				User fetchedUser = userService.GetUserById(1);

				Assert.AreEqual(user.Id, fetchedUser.Id);
				Assert.AreEqual(user.FirstName, fetchedUser.FirstName);
			}
		}

		[TestMethod]
		public void UserService_UpdateUser_Success()
		{
			User user = new User("Inigo", "Montoya");

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(user);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				user.FirstName = "Tom";
				user.LastName = "Holland";
				userService.UpdateUser(user);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				User updatedUser = userService.GetUserById(user.Id);

				Assert.AreEqual("Tom", updatedUser.FirstName);
			}
		}

		[TestMethod]
		public void UserService_GetAllUsers_Success()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(new User("Inigo", "Montoya"));
				userService.AddUser(new User("Princess", "Buttercup"));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				List<User> allUsers = userService.GetAllUsers();

				Assert.AreEqual(2, allUsers.Count);
				Assert.AreEqual("Inigo", allUsers[0].FirstName);
				Assert.AreEqual("Buttercup", allUsers[1].LastName);
			}
		}

		[TestCleanup]
		public void ResetUserIdCounter()
		{
			User.ResetCounter();
		}
	}
}
