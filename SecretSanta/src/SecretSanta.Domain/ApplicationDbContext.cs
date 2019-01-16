using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }
		//public DbSet<Gift> Gifts { get; set; }
		//public DbSet<Pairing> Pairings { get; set; }
		//public DbSet<Message> Messages { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserGroup>()
				.HasKey(t => new { t.UserId, t.GroupId });
		}

		/* Doing it this way prevents us from having a different DB for testing vs production
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("DataSource:memory");
			base.OnConfiguring(optionsBuilder);
		}
		*/
	}
}
