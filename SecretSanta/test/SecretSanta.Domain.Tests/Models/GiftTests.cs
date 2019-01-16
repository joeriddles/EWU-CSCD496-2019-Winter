using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class GiftTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Gift_CreateGiftWithNull_ExpectException()
		{
			Gift gift = new Gift(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Gift_CreateGiftWithInvalidId_ExpectException()
		{
			Gift gift = new Gift("Car", "Vroom vroom!", -1);
		}

		[TestMethod]
		public void Gift_CreateGift_Success()
		{
			Gift gift = new Gift(title: "Car", description: "Vroom vroom!", userId: 1, orderOfImportance:1, url: "http://www.car.com/");
			Assert.AreEqual(1, gift.Id);
			Assert.AreEqual("Car", gift.Title);
			Assert.AreEqual("Vroom vroom!", gift.Description);
			Assert.AreEqual(1, gift.UserId);
			Assert.AreEqual(1, gift.OrderOfImportance);
			Assert.AreEqual("http://www.car.com/", gift.Url);
		}
	}
}
