using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Models.GroupPermission> GroupPermissions { get; set; }
        public DbSet<Models.Permission> Permissions { get; set; }
        public DbSet<Models.UserGroup> UserGroups { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GroupPermission>()
                .HasKey(gp => new { gp.GroupId, gp.PermissionId });

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Group)
                .WithMany(g => g.Permissions)
                .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany(p => p.Groups)
                .HasForeignKey(gp => gp.PermissionId);

            modelBuilder.Entity<UserGroup>()
                .HasKey(gp => new { gp.UserId, gp.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(gp => gp.User)
                .WithMany(g => g.Groups)
                .HasForeignKey(gp => gp.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(gp => gp.Group)
                .WithMany(p => p.Users)
                .HasForeignKey(gp => gp.GroupId);
        }
    }
}
