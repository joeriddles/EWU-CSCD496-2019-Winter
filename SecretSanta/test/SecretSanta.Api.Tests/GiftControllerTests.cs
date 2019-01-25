using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
	[TestClass]
	public class GiftControllerTests
	{
		/*
		 * 3 different cases to tests:
		 *	1) controller checking for null -> throws exception
		 *	2) userId less than/equal to 0
		 *	3) 
		 */
		[TestMethod]
		public void GetGiftForUser_NegativeUserId_ReturnsNotFound()
		{
			var testService = new TestService();
			var controller = new GiftController(testService);
			ActionResult<List<Gift>> result = controller.GetGiftForUser(-1);
			Assert.IsTrue(result is NotFoundResult);
		}
	}

	public class TestService : IGiftService
	{
		public List<Domain.Models.Gift> GetGiftsForUser(int userId)
		{
			return new List<Domain.Models.Gift>();
		}
	}
}
