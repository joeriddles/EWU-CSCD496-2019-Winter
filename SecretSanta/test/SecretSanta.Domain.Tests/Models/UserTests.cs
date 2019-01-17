using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class UserTests : BaseModelTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void User_CreateUserWithNull_ExpectException()
		{
			User user = new User(null, null);
		}

		[TestMethod]
		public void User_CreateUser_Success()
		{
			User user = new User("Inigo", "Montoya");
			Assert.AreEqual("Inigo", user.FirstName);
			Assert.AreEqual("Montoya", user.LastName);
			Assert.IsNotNull(user.UserGroups);
			Assert.IsNotNull(user.Gifts);
		}
	}
}
 