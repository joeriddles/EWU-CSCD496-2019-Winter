using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public class PairingServiceTests : BaseTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PairingService_AddNullPairing_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				pairingService.AddPairing(null);
			}
		}

		[TestMethod]
		public void PairingService_GetPairingThatDoesNotExist_ExpectNull()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				Pairing pairing = pairingService.GetPairingById(-1);

				Assert.IsNull(pairing);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PairingService_UpdatePairingWithNull_ExpectException()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				pairingService.UpdatePairing(null);
			}
		}

		[TestMethod]
		public void PairingService_AddPairing_Success()
		{
			Pairing pairing = new Pairing(1, 2);
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				int oldCount = pairingService.GetPairingCount();
				pairingService.AddPairing(pairing);

				Assert.AreEqual(pairingService.GetPairingCount(), oldCount + 1);
			}
		}

		[TestMethod]
		public void PairingService_GetPairing_Success()
		{
			Pairing pairing = new Pairing(1, 2);
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				pairingService.AddPairing(pairing);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				Pairing fetchedPairing = pairingService.GetPairingById(pairing.Id);

				Assert.AreEqual(pairing.Id, fetchedPairing.Id);
				Assert.AreEqual(pairing.RecipientId, fetchedPairing.RecipientId);
				Assert.AreEqual(pairing.SantaId, fetchedPairing.SantaId);
			}
		}

		[TestMethod]
		public void PairingService_UpdatePairing_Success()
		{
			Pairing pairing = new Pairing(1, 2);
			Group group = new Group(".NET User group");
			using (var context = new ApplicationDbContext(Options))
			{
				GroupService groupService = new GroupService(context);
				groupService.AddGroup(group);

				PairingService pairingService = new PairingService(context);
				pairingService.AddPairing(pairing);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				pairing.GroupId = 1;
				pairingService.UpdatePairing(pairing);
			}

			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				Pairing updatedPairing = pairingService.GetPairingById(pairing.Id);

				Assert.AreEqual(group.Id, updatedPairing.GroupId);
			}
		}

		[TestMethod]
		public void PairingService_GetAllPairings_Success()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				pairingService.AddPairing(new Pairing(1, 2));
				pairingService.AddPairing(new Pairing(2, 1));
			}

			using (var context = new ApplicationDbContext(Options))
			{
				PairingService pairingService = new PairingService(context);
				List<Pairing> allPairings = pairingService.GetAllPairings();

				Assert.AreEqual(2, allPairings.Count);
				Assert.AreEqual(1, allPairings[0].RecipientId);
				Assert.AreEqual(2, allPairings[1].RecipientId);
			}
		}

		[TestInitialize]
		public void Initialize()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.AddUser(new User("General", "Kenobi"));
				userService.AddUser(new User("General", "Grevious"));
			}
		}
	}
}
