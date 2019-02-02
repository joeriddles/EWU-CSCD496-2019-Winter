using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
	[TestClass]
	public class GroupControllerTests
	{
		private Mock<IGroupService> groupService;
		private Mock<IGroupUserService> groupUserService;
		private AutoMocker mocker;

		private readonly Group modelGroup = new Group()
		{
			Id = 1,
			Name = ".NET User Group"
		};

		private readonly DTO.Group dtoGroup = new DTO.Group()
		{
			Id = 1,
			Name = ".NET User Group"
		};

		private readonly GroupUser groupUser = new GroupUser()
		{
			GroupId = 1,
			UserId = 1
		};

		private readonly User modelUser = new User()
		{
			Id = 1,
			FirstName = "John",
			LastName = "Smith"
		};

		private readonly DTO.User dtoUser = new DTO.User()
		{
			Id = 1,
			FirstName = "John",
			LastName = "Smith"
		};

		[TestInitialize]
		public void TestInit()
		{
			mocker = new AutoMocker();
			groupService = mocker.GetMock<IGroupService>();

			groupService.Setup(us => us.GetGroup(1))
				.Returns(modelGroup)
				.Verifiable();

			groupService.Setup(us => us.AddGroup(It.IsAny<Group>()))
				.Returns(modelGroup)
				.Verifiable();

			groupService.Setup(us => us.UpdateGroup(It.IsAny<Group>()))
				.Returns(modelGroup)
				.Verifiable();

			groupService.Setup(us => us.RemoveGroup(It.IsAny<Group>()))
				.Verifiable();

			groupService.Setup(us => us.GetAllGroups())
				.Returns(new List<Group> {modelGroup})
				.Verifiable();

			groupUserService = mocker.GetMock<IGroupUserService>();

			groupUserService.Setup(gus => gus.GetGroupUser(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(groupUser)
				.Verifiable();

			groupUserService.Setup(gus => gus.AddGroupUser(It.IsAny<GroupUser>()))
				.Returns(groupUser)
				.Verifiable();

			groupUserService.Setup(gus => gus.RemoveGroupUser(It.IsAny<GroupUser>()))
				.Verifiable();

			groupUserService.Setup(gus => gus.GetAllUsersInGroup(It.IsAny<int>()))
				.Returns(new List<User> {modelUser})
				.Verifiable();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GroupController_RequiresGroupService()
		{
			new GroupController(null, groupUserService.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GroupController_RequiresGroupUserService()
		{
			new GroupController(groupService.Object, null);
		}

		[TestMethod]
		public void GetGroup_InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.GetGroup(-1);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void GetGroup_GroupId1_ReturnsListOfGroups()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			groupController.GetGroup(1);
			groupService.Verify(gs => gs.GetGroup(1));
		}

		[TestMethod]
		public void GetAllGroups_ReturnsListOfGroups()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			groupController.GetAllGroups();
			groupService.Verify(gs => gs.GetAllGroups());
		}

		[TestMethod]
		public void AddGroup_ListOfGroupIsNull_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.AddGroup(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddGroup_InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.AddGroup(new DTO.Group());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddGroup_GroupId1_ReturnsListOfGroups()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> result = groupController.AddGroup(dtoGroup);
			groupService.Verify(gs => gs.AddGroup(It.IsAny<Group>()));
		}

		[TestMethod]
		public void UpdateGroup_ListOfGroupIsNull_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.UpdateGroup(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateGroup_InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.UpdateGroup(new DTO.Group());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateGroup_GroupId1_ReturnsListOfGroups()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> result = groupController.UpdateGroup(dtoGroup);
			groupService.Verify(gs => gs.UpdateGroup(It.IsAny<Group>()));
		}

		[TestMethod]
		public void RemoveGroup_ListOfGroupIsNull_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> response = groupController.RemoveGroup(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void RemoveGroup_GroupId1_ReturnsListOfGroups()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<DTO.Group> result = groupController.RemoveGroup(dtoGroup);
			groupService.Verify(gs => gs.RemoveGroup(It.IsAny<Group>()));
		}

		// GroupUsers

		[TestMethod]
		public void GetAllUsersInGroup__InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.GetAllUsersInGroup(-1);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void GetAllUsersInGroup__GroupId1_ExpectOk()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.GetAllUsersInGroup(1);
			groupUserService.Verify(gus => gus.GetAllUsersInGroup(It.IsAny<int>()));
		}

		[TestMethod]
		public void AddUserToGroup__InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.AddUserToGroup(-1, dtoUser);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddUserToGroup__UserIsNull_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.AddUserToGroup(1, null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddUserToGroup__GroupId1_ExpectOk()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.AddUserToGroup(1, dtoUser);
			groupUserService.Verify(gus => gus.AddGroupUser(It.IsAny<GroupUser>()));
		}

		[TestMethod]
		public void RemoveUserFromGroup__InvalidGroupId_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.RemoveUserFromGroup(-1, dtoUser);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void RemoveUserFromGroup__UserIsNull_ExpectsBadRequest()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.RemoveUserFromGroup(1, null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void RemoveUserFromGroup__GroupId1_ExpectOk()
		{
			GroupController groupController = new GroupController(groupService.Object, groupUserService.Object);
			ActionResult<List<DTO.User>> response = groupController.RemoveUserFromGroup(1, dtoUser);
			groupUserService.Verify(gus => gus.RemoveGroupUser(It.IsAny<GroupUser>()));
		}
	}
}
