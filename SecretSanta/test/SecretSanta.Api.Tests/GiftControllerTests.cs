using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using Moq;
using Moq.AutoMock;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
	[TestClass]
	public class GiftControllerTests
	{
		private Mock<IGiftService> giftService;
		private AutoMocker mocker;

		private readonly List<DTO.Gift> dtoGifts = new List<DTO.Gift>()
		{
			new DTO.Gift() {Id = 1, Title = "Xbox", Description = "Gaming system"},
			new DTO.Gift() {Id = 2, Title = "Car", Description = "Vroom vroom!"}
		};

		private readonly List<Gift> modelGifts = new List<Gift>()
		{
			new Gift() {Id = 1, Title = "Xbox", Description = "Gaming system"},
			new Gift() {Id = 2, Title = "Car", Description = "Vroom vroom!"}
		};

		private readonly Gift modelGift = new Gift() { Id = 3, Title = "Toy", Description = "Priceless" };

		[TestInitialize]
		public void TestInit()
		{
			mocker = new AutoMocker();
			giftService = mocker.GetMock<IGiftService>();

			giftService.Setup(gs => gs.GetGiftsForUser(1))
				.Returns(modelGifts)
				.Verifiable();

			giftService.Setup(gs => gs.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()))
				.Returns(modelGift)
				.Verifiable();

			giftService.Setup(gs => gs.UpdateGiftForUser(It.IsAny<int>(), It.IsAny<Gift>()))
				.Returns(modelGift)
				.Verifiable();

			giftService.Setup(gs => gs.RemoveGift(It.IsAny<Gift>()))
				.Verifiable();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiftController_RequiresGiftService()
		{
			new GiftController(null);
		}

		[TestMethod]
		public void GetGiftsForUser_InvalidUserId_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.GetGiftsForUser(-1);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void GetGiftsForUser_UserId1_ReturnsListOfGifts()
		{
			GiftController giftController = new GiftController(giftService.Object);
			giftController.GetGiftsForUser(1);
			giftService.Verify(gs => gs.GetGiftsForUser(1));
		}

		[TestMethod]
		public void AddGiftsForUser_ListOfGiftsIsNull_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.AddGiftsToUser(-1, null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddGiftsToUser_InvalidUserId_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.AddGiftsToUser(-1, new List<DTO.Gift>());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void AddGiftsToUser_UserId1_ReturnsListOfGifts()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> result = giftController.AddGiftsToUser(1, dtoGifts);
			giftService.Verify(gs => gs.AddGiftToUser(1, It.IsAny<Gift>()));
		}

		[TestMethod]
		public void UpdateGiftsForUser_ListOfGiftsIsNull_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.UpdateGiftsForUser(-1, null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateGiftsForUser_InvalidUserId_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.UpdateGiftsForUser(-1, new List<DTO.Gift>());
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void UpdateGiftsToUser_UserId1_ReturnsListOfGifts()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> result = giftController.UpdateGiftsForUser(1, dtoGifts);
			giftService.Verify(gs => gs.UpdateGiftForUser(1, It.IsAny<Gift>()));
		}

		[TestMethod]
		public void RemoveGifts_ListOfGiftsIsNull_ExpectsBadRequest()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> response = giftController.RemoveGifts(null);
			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public void RemoveGifts_UserId1_ReturnsListOfGifts()
		{
			GiftController giftController = new GiftController(giftService.Object);
			ActionResult<List<DTO.Gift>> result = giftController.RemoveGifts(dtoGifts);
			giftService.Verify(gs => gs.RemoveGift(It.IsAny<Gift>()));
		}

	}
}