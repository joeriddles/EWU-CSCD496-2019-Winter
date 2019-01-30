using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Moq.AutoMock;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
	[TestClass]
	public class GiftControllerTests
	{
		[ClassInitialize]
		public void ClassInit()
		{
			var mocker = new AutoMocker();
			var giftController = mocker.CreateInstance<GiftController>();
			var giftService = mocker.GetMock<IGiftService>();
			// do not need TestableGiftService if mocking IGiftService...?
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiftController_RequiresGiftService()
		{
			new GiftController(null);
		}

		[TestMethod]
		public void GetGiftForUser_ReturnsUsersFromService()
		{
			var gift = new Gift
			{
				Id = 3,
				Title = "Gift Tile",
				Description = "Gift Description",
				Url = "http://www.gift.url",
				OrderOfImportance = 1
			};
			var testService = new TestableGiftService
			{
				ListToReturn = new List<Gift>
				{
					gift
				}
			};
			var controller = new GiftController(testService);

			ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(4);

			Assert.AreEqual(4, testService.UserId);
			DTO.Gift resultGift = result.Value.Single();
			Assert.AreEqual(gift.Id, resultGift.Id);
			Assert.AreEqual(gift.Title, resultGift.Title);
			Assert.AreEqual(gift.Description, resultGift.Description);
			Assert.AreEqual(gift.Url, resultGift.Url);
			Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);
		}

		[TestMethod]
		public void GetGiftForUser_RequiresPositiveUserId()
		{
			var testService = new TestableGiftService();
			var controller = new GiftController(testService);

			ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(-1);

			Assert.IsTrue(result.Result is BadRequestResult);
			//This check ensures that the service was not called
			Assert.AreEqual(0, testService.UserId);
		}

		[TestMethod]
		public void TestMock()
		{
			var temp = new Mock<IGiftService>(); //more garbo
		}
	}
}