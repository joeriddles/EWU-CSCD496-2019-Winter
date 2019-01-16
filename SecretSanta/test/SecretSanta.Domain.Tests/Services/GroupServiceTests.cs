using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class GroupServiceTests : BaseTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GroupService_AddNullGroup_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.AddGroup(null);
			}
		}

		[TestMethod]
		public void GroupService_GetGroupThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				Group group = groupService.GetGroupById(-1);

				Assert.IsNull(group);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GroupService_UpdateGroupWithNull_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.UpdateGroup(null);
			}
		}

		[TestMethod]
		public void GroupService_AddGroup_Success()
		{
			Group group = new Group(".NET User group");

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				int oldCount = groupService.GetGroupCount();
				groupService.AddGroup(group);

				Assert.AreEqual(groupService.GetGroupCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void GroupService_GetGroup_Success()
		{
			Group group = new Group(".NET User group");
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.AddGroup(group);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				Group fetchedGroup = groupService.GetGroupById(1);

				Assert.AreEqual(group.Id, fetchedGroup.Id);
				Assert.AreEqual(group.Title, fetchedGroup.Title);
			}
		}

		[TestMethod]
		public void GroupService_UpdateGroup_Success()
		{
			Group group = new Group(".NET User group");

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.AddGroup(group);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				group.Title = "Java User group";
				groupService.UpdateGroup(group);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				Group updatedGroup = groupService.GetGroupById(group.Id);

				Assert.AreEqual("Java User group", updatedGroup.Title);
			}
		}

		[TestMethod]
		public void GroupService_GetAllGroups_Success()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.AddGroup(new Group(".NET User group"));
				groupService.AddGroup(new Group("Java User group"));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				List<Group> allGroups = groupService.GetAllGroups();

				Assert.AreEqual(2, allGroups.Count);
				Assert.AreEqual(".NET User group", allGroups[0].Title);
				Assert.AreEqual("Java User group", allGroups[1].Title);
			}
		}

		[TestCleanup]
		public void ResetGroupIdCounter()
		{
			Group.ResetCounter();
		}
	}
}
