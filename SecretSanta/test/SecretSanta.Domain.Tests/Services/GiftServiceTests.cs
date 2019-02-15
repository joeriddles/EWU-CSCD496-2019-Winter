using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class GiftServiceTests : DatabaseServiceTests
    {
        [TestMethod]
        public void AddGift()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = userService.AddUser(user).Result;

                var gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1
                };

                var persistedGift = giftService.AddGiftToUser(user.Id, gift);

                Assert.AreNotEqual(0, persistedGift.Id);
            }
        }

        [TestMethod]
        public void UpdateGift()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = userService.AddUser(user).Result;

                var gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1
                };

                var persistedGift = giftService.AddGiftToUser(user.Id, gift);

                Assert.AreNotEqual(0, persistedGift.Id);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                var users = userService.FetchAll();
                var gifts = giftService.GetGiftsForUser(users.Result[0].Id);

                Assert.IsTrue(gifts.Result.Count > 0);

                gifts.Result[0].Title = "Horse";
				// Assign result of UpdateGiftForUser(...).Result to variable to make sure it finishes before code continues
				var wait = giftService.UpdateGiftForUser(users.Result[0].Id, gifts.Result[0]).Result;
            }

            using (var context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                var users = userService.FetchAll();
                var gifts = giftService.GetGiftsForUser(users.Result[0].Id);

                Assert.IsTrue(gifts.Result.Count > 0);
                Assert.AreEqual("Horse", gifts.Result[0].Title);            
            }
        }
    }
}