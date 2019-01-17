using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class GiftServiceTests : BaseServiceTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiftService_AddNullGift_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				giftService.AddGift(null);
			}
		}

		[TestMethod]
		public void GiftService_GetGiftThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				Gift gift = giftService.GetGiftById(-1);

				Assert.IsNull(gift);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiftService_UpdateGiftWithNull_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				giftService.UpdateGift(null);
			}
		}

		[TestMethod]
		public void GiftService_AddGift_Success()
		{
			Gift gift = new Gift("Car", "Vroom vroom!", 1, 1, "http://car.com");
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				int oldCount = giftService.GetGiftCount();
				giftService.AddGift(gift);

				Assert.AreEqual(giftService.GetGiftCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void GiftService_GetGift_Success()
		{
			Gift gift = new Gift("Car", "Vroom vroom!", 1, 1, "http://car.com");
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				giftService.AddGift(gift);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				Gift fetchedGift = giftService.GetGiftById(gift.Id);

				Assert.AreEqual(gift.Id, fetchedGift.Id);
				Assert.AreEqual(gift.Title, fetchedGift.Title);
			}
		}

		[TestMethod]
		public void GiftService_UpdateGift_Success()
		{
			Gift gift = new Gift("Car", "Vroom vroom!", 1, 1, "http://car.com");
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				giftService.AddGift(gift);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				gift.Title = "Power Ranger";
				giftService.UpdateGift(gift);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				Gift updatedGift = giftService.GetGiftById(gift.Id);

				Assert.AreEqual("Power Ranger", updatedGift.Title);
			}
		}

		[TestMethod]
		public void GiftService_GetAllGifts_Success()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				giftService.AddGift(new Gift("Car", "Vroom vroom!", 1, 1, "http://car.com"));
				giftService.AddGift(new Gift("Power Ranger", "This is the blue one.", 1, 2, "http://www.powerranger.com"));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				GiftService giftService = new GiftService(context);
				List<Gift> allGifts = giftService.GetAllGifts();

				Assert.AreEqual(2, allGifts.Count);
				Assert.AreEqual("Car", allGifts[0].Title);
				Assert.AreEqual("Power Ranger", allGifts[1].Title);
			}
		}

		[TestInitialize]
		public void Initialize()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(new User("Inigo", "Montoya"));
			}
		}
	}
}
