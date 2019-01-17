using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class UserGroupTests : BaseModelTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UserGroup_CreateUserGroupWithNull_ExpectException()
		{
			UserGroup userGroup = new UserGroup(null, null);
		}

		[TestMethod]
		public void UserGroup_CreateUserGroup_Success()
		{
			User user = new User("Inigo", "Montoya");
			Group group = new Group(".NET User group");

			UserGroup userGroup = new UserGroup(user, group);
			Assert.AreEqual("Inigo", userGroup.User.FirstName);
			Assert.AreEqual("Montoya", userGroup.User.LastName);
			Assert.AreEqual(".NET User group", userGroup.Group.Title);
		}
	}
}
