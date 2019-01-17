using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
	[TestClass]
	public abstract class BaseTest
	{
		[TestInitialize]
		public void OpenConnection()
		{
			SqliteConnection = new SqliteConnection("DataSource=:memory:");
			SqliteConnection.Open();

			Options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseSqlite(SqliteConnection)
				.Options;

			using (var context = new ApplicationDbContext(Options))
			{
				context.Database.EnsureCreated();
			}
		}

		[TestCleanup]
		public void CloseConnection()
		{
			using (var context = new ApplicationDbContext(Options))
			{
				UserService userService = new UserService(context);
				userService.DeleteAllUsers();

				GroupService groupService = new GroupService(context);
				groupService.DeleteAllGroups();

				UserGroupService userGroupService = new UserGroupService(context);
				userGroupService.DeleteAllUserGroups();
			}

			SqliteConnection.Close();

			User.ResetCounter();
			Group.ResetCounter();
			Gift.ResetCounter();
			Message.ResetCounter();
			Pairing.ResetCounter();
		}

		protected SqliteConnection SqliteConnection { get; set; }
		protected DbContextOptions<ApplicationDbContext> Options { get; set; }
	}
}
