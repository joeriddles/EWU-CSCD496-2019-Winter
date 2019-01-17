using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class GroupTests : BaseModelTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Group_CreateGroupWithNull_ExpectException()
		{
			Group group = new Group(null);
		}

		[TestMethod]
		public void CreateGroup()
		{
			Group group = new Group(".NET usergroup");
			Assert.AreEqual(".NET usergroup", group.Title);
			Assert.IsNotNull(group.UserGroups);
		}
	}
}
