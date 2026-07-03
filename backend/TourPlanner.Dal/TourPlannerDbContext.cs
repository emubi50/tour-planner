using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TourPlanner.Models;

namespace TourPlanner.Dal
{
    public class TourPlannerDbContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }

        public DbSet<User> Users { get; set; }

        public TourPlannerDbContext(DbContextOptions<TourPlannerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Tour>()
                .HasMany(t => t.Logs)
                .WithOne(l => l.Tour)
                .HasForeignKey(l => l.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Tour>(entity =>
                {
                    entity.Property(t => t.Popularity)
                        .HasConversion<string>()
                        .HasMaxLength(20)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    entity.Property(t => t.ChildFriendliness)
                        .HasConversion<string>()
                        .HasMaxLength(20)
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    entity.Property(t => t.SearchVector)
                        .HasColumnType("tsvector")
                        .ValueGeneratedOnAddOrUpdate()
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    entity.Property(t => t.TransportType)
                        .HasConversion<string>()
                        .HasMaxLength(20);
                });

            modelBuilder.Entity<TourLog>()
                .Property(t => t.Date)
                .HasColumnType("date");

            modelBuilder.Entity<Tour>().OwnsOne(t => t.From);
            modelBuilder.Entity<Tour>().OwnsOne(t => t.To);
        }
    }
}
