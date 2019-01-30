using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
	public class TestableGiftService : IGiftService
	{
		public List<Gift> ListToReturn { get; set; }
		public Gift GiftToReturn { get; set; }
		public int UserId { get; set; }
		public Gift Gift { get; set; }


		public List<Gift> GetGiftsForUser(int userId)
		{
			UserId = userId;
			return ListToReturn;
		}

		public Gift AddGiftToUser(int userId, Gift gift)
		{
			UserId = userId;
			Gift = gift;
			return GiftToReturn;
		}

		public Gift UpdateGiftForUser(int userId, Gift gift)
		{
			Gift = gift;
			Gift.Id = userId;
			return GiftToReturn;
		}

		public void RemoveGift(Gift gift)
		{
			ListToReturn.Remove(gift);
		}
	}
}