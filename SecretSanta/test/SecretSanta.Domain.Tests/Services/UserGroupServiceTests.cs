using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class UserGroupServiceTests : BaseServiceTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UserGroupService_AddNullUserGroup_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserGroupService userGroupService = new UserGroupService(context);
				userGroupService.AddUserGroup(null);
			}
		}

		[TestMethod]
		public void UserGroupService_GetUserGroupThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserGroupService userGroupService = new UserGroupService(context);
				UserGroup userGroup = userGroupService.GetUserGroupByIds(-1, -1);

				Assert.IsNull(userGroup);
			}
		}

		[TestMethod]
		public void UserGroupService_AddUserGroup_Success()
		{
			User user = new User("Inigo", "Montoya");
			Group group = new Group(".NET User group");
			UserGroup userGroup = new UserGroup(user, group);

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(user);

				GroupService groupService = new GroupService(context);
				groupService.AddGroup(group);

				UserGroupService userGroupService = new UserGroupService(context);
				int oldCount = userGroupService.GetUserGroupCount();
				userGroupService.AddUserGroup(userGroup);

				Assert.AreEqual(userGroupService.GetUserGroupCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void UserGroupService_GetUserGroup_Success()
		{
			UserGroup userGroup = new UserGroup(_user, _group);

			using (var context = new ApplicationDbContext(Options))
			{
				// These four lines are necessary, not sure why?
				UserService userService = new UserService(context);
				GroupService groupService = new GroupService(context);
				var users = userService.GetAllUsers();
				var groups = groupService.GetAllGroups();

				UserGroupService userGroupService = new UserGroupService(context);
				var userGroups = userGroupService.GetAllUserGroups();

				userGroupService.AddUserGroup(userGroup);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserGroupService userGroupService = new UserGroupService(context);
				UserGroup fetchedUserGroup = userGroupService.GetUserGroupByIds(userGroup.UserId, userGroup.GroupId);

				Assert.AreEqual(userGroup.UserId, fetchedUserGroup.UserId);
				Assert.AreEqual(userGroup.GroupId, fetchedUserGroup.GroupId);
			}
		}

		[TestMethod]
		public void UserGroupService_GetAllUserGroups_Success()
		{
			User secondUser = new User("Princess", "Buttercup");
			Group secondGroup = new Group("Java User group");

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(secondUser);

				GroupService groupService = new GroupService(context);
				groupService.AddGroup(secondGroup);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				// These four lines are necessary, not sure why?
				UserService userService = new UserService(context);
				GroupService groupService = new GroupService(context);
				var users = userService.GetAllUsers();
				var groups = groupService.GetAllGroups();

				UserGroupService userGroupService = new UserGroupService(context);
				var userGroups = userGroupService.GetAllUserGroups();


				userGroupService.AddUserGroup(new UserGroup(_user, _group));
				userGroupService.AddUserGroup(new UserGroup(secondUser, secondGroup));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				UserGroupService userGroupService = new UserGroupService(context);
				List<UserGroup> allUserGroups = userGroupService.GetAllUserGroups();

				Assert.AreEqual(2, allUserGroups.Count);
				Assert.AreEqual(_user.Id, allUserGroups[0].UserId);
				Assert.AreEqual(secondUser.Id, allUserGroups[1].UserId);
				Assert.AreEqual(_group.Id, allUserGroups[0].GroupId);
				Assert.AreEqual(secondGroup.Id, allUserGroups[1].GroupId);
			}
		}

		[TestMethod]
		public void UserGroup_EagerlyVsNotEagerlyLoad()
		{
			User user = new User("Inigo", "Montoya");
			Group group = new Group(".NET User group");
			UserGroup userGroup = new UserGroup(user, group);

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(user);

				GroupService groupService = new GroupService(context);
				groupService.AddGroup(group);

				UserGroupService userGroupService = new UserGroupService(context);
				userGroupService.AddUserGroup(userGroup);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				List<UserGroup> fullUserGroups = context.UserGroups
					.ToList();
				Assert.IsNull(fullUserGroups.Single().User);
				Assert.IsNull(fullUserGroups.Single().Group);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				List<UserGroup> fullUserGroups = context.UserGroups
					.Include(ug => ug.User)
					.Include(ug => ug.Group)
					.ToList();
				Assert.IsNotNull(fullUserGroups.Single().User);
				Assert.IsNotNull(fullUserGroups.Single().Group);
			}
		}

		[TestInitialize]
		public void Initialize()
		{
			_user = new User("Inigo", "Montoya");
			_group = new Group(".NET User group");

			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(_user);

				GroupService groupService = new GroupService(context);
				groupService.AddGroup(_group);
			}
		}

		private User _user;
		private Group _group;
	}
}
