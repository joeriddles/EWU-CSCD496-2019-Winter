using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class PairingTests
	{
		[TestMethod]
		public void CreatePairing()
		{
			Pairing pairing = new Pairing(recipientId: 1, santaId:2, groupId:3);
			Assert.AreEqual(1, pairing.RecipientId);
			Assert.AreEqual(2, pairing.SantaId);
			Assert.AreEqual(3, pairing.GroupId);
		}
	}
}
