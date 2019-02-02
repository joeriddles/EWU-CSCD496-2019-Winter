using System;
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
	public class UserControllerTests
	{
		private Mock<IUserService> userService;
		private AutoMocker mocker;

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
			userService = mocker.GetMock<IUserService>();

			userService.Setup(us => us.GetUser(1))
				.Returns(modelUser)
				.Verifiable();

			userService.Setup(us => us.AddUser(It.IsAny<User>()))
				.Returns(modelUser)
				.Verifiable();

			userService.Setup(us => us.UpdateUser(It.IsAny<User>()))
				.Returns(modelUser)
				.Verifiable();

			userService.Setup(us => us.RemoveUser(It.IsAny<User>()))
				.Verifiable();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UserController_RequiresUserService()
		{
			new UserController(null);
		}

		[TestMethod]
		public void GetUser_InvalidUserId_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.GetUser(-1);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void GetUser_UserId1_ReturnsListOfUser()
		{
			UserController giftController = new UserController(userService.Object);
			giftController.GetUser(1);
			userService.Verify(gs => gs.GetUser(1));
		}

		[TestMethod]
		public void AddUser_ListOfUserIsNull_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.AddUser(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddUser_InvalidUserId_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.AddUser(new DTO.User());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddUser_UserId1_ReturnsListOfUser()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> result = giftController.AddUser(dtoUser);
			userService.Verify(gs => gs.AddUser(It.IsAny<User>()));
		}

		[TestMethod]
		public void UpdateUser_ListOfUserIsNull_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.UpdateUser(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateUser_InvalidUserId_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.UpdateUser(new DTO.User());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateUser_UserId1_ReturnsListOfUser()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> result = giftController.UpdateUser(dtoUser);
			userService.Verify(gs => gs.UpdateUser(It.IsAny<User>()));
		}

		[TestMethod]
		public void RemoveUser_ListOfUserIsNull_ExpectsBadRequest()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> response = giftController.RemoveUser(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void RemoveUser_UserId1_ReturnsListOfUser()
		{
			UserController giftController = new UserController(userService.Object);
			ActionResult<DTO.User> result = giftController.RemoveUser(dtoUser);
			userService.Verify(gs => gs.RemoveUser(It.IsAny<User>()));
		}
	}
}
