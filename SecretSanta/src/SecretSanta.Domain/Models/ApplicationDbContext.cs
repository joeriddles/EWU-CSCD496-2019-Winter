using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Domain.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Pairing> Pairings { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
			// Real way: EF Migrations with real file...? but this is in memory so YOLO
	        Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupUser>().HasKey(gu => new { gu.UserId, gu.GroupId });

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(gu => gu.UserId);

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupUsers)
                .HasForeignKey(gu => gu.GroupId);
        }
    }
}