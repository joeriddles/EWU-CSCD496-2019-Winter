using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class GiftService
	{
		private ApplicationDbContext DbContext { get; }

		public GiftService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		//CRUD: Create, Read, Update, Delete

		public void AddGift(Gift gift)
		{
			DbContext.Gifts.Add(gift);
			DbContext.SaveChanges();
		}

		public Gift GetGiftById(int giftId)
		{
			return DbContext.Gifts.Find(giftId);
		}

		public List<Gift> GetAllGifts()
		{
			return DbContext.Set<Gift>().ToList();
		}

		public void UpdateGift(Gift gift)
		{
			DbContext.Gifts.Update(gift);
			DbContext.SaveChanges();
		}

		public int GetGiftCount()
		{
			return DbContext.Gifts.Count();
		}
	}
}
