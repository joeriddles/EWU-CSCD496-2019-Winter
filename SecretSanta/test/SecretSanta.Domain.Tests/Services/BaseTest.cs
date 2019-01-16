using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

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
			SqliteConnection.Close();
		}

		protected SqliteConnection SqliteConnection { get; set; }
		protected DbContextOptions<ApplicationDbContext> Options { get; set; }
	}
}
