using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApplication.Models.Contexts
{
    public class WebApplicationContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public WebApplicationContext(DbContextOptions<WebApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("WebApplication.Models.Feedback", b =>
            {
                b.HasOne("WebApplication.Models.User", "User")
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("WebApplication.Models.Vote", b =>
            {
                b.HasOne("WebApplication.Models.User", "User")
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("WebApplication.Models.Vote", b =>
            {
                b.HasOne("WebApplication.Models.Feedback", "Feedback")
                .WithMany()
                .HasForeignKey("FeedbackId")
                .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
