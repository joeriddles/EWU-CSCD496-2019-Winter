using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class UserTests
	{
		[TestMethod]
		public void CreateUser()
		{
			User user = new User("Inigo", "Montoya");
			Assert.AreEqual(1, user.Id);
			Assert.AreEqual("Inigo", user.FirstName);
			Assert.AreEqual("Montoya", user.LastName);
			Assert.IsNotNull(user.UserGroups);
			Assert.IsNotNull(user.Gifts);
		}
	}
}
 