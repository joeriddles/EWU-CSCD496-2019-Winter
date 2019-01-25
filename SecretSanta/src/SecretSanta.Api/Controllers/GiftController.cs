using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GiftController : ControllerBase
	{
		// interfaces used as a way to hijack calls -> easier to unit test
		private readonly IGiftService _giftService;
		
		public GiftController(IGiftService giftService)
		{
			_giftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
		}

		// GET api/values/5
		[HttpGet("{userId}")]
		public ActionResult<List<Gift>> GetGiftForUser(int userId)
		{
			if (userId <= 0)
				return base.NotFound();

			// Implicit operators: converts to ActionResult
			return _giftService.GetGiftsForUser(userId)
				.Select(modelGift => new Gift(modelGift))
				.ToList();
		}
	}
}
