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
		private readonly IGiftService _giftService;

		public GiftController(IGiftService giftService)
		{
			_giftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
		}

		// GET api/Gift/5
		[HttpGet("{userId}")]
		public ActionResult<List<Gift>> GetGiftsForUser(int userId)
		{
			if (userId <= 0) return BadRequest("userId invalid.");

			return _giftService.GetGiftsForUser(userId)
				.Select(modelGift => new Gift(modelGift))
				.ToList();
		}

		// POST api/Gift/5
		[HttpPost("{userId}")]
		public ActionResult<List<Gift>> AddGiftsToUser(int userId, List<Gift> gifts)
		{
			if (gifts is null) return BadRequest("gifts is null.");
			if (userId <= 0) return BadRequest("userId invalid.");

			return Created(
				$"api/Gift/{userId}",
				gifts.Select(gift =>
					new Gift(_giftService.AddGiftToUser(userId, Gift.ToDomain(gift)))
				).ToList()
			);
		}

		// PUT api/Gift/5
		[HttpPut("{userId}")]
		public ActionResult<List<Gift>> UpdateGiftsForUser(int userId, List<Gift> gifts)
		{
			if (gifts is null) return BadRequest("gifts is null.");
			if (userId <= 0) return BadRequest("userId invalid.");

			return gifts.Select(gift =>
				new Gift(_giftService.UpdateGiftForUser(userId, Gift.ToDomain(gift)))
			).ToList();
		}

		// DELETE api/Gift
		[HttpDelete]
		public ActionResult RemoveGifts(List<Gift> gifts)
		{
			if (gifts is null) return BadRequest("gifts is null.");

			gifts.ForEach(gift =>
				_giftService.RemoveGift(Gift.ToDomain(gift))
			);
			return NoContent(); // 204 status code: success but no content
		}
	}
}