using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
	[TestClass]
	public class BaseModelTest
	{
		[TestCleanup]
		public void Cleanup()
		{
			User.ResetCounter();
			Group.ResetCounter();
			Gift.ResetCounter();
			Message.ResetCounter();
			Pairing.ResetCounter();
		}
	}
}
