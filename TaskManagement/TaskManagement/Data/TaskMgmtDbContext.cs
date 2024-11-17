using Microsoft.EntityFrameworkCore;
using TaskManagement.Model;

namespace TaskManagement.Data
{
    public class TaskMgmtDbContext : DbContext
    {
        public TaskMgmtDbContext(DbContextOptions<TaskMgmtDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskEntry> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntry>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
