namespace VebTechTestTask.DAL
{
    using System.Reflection;
    
    using Entities;

    using Microsoft.EntityFrameworkCore;

    public class VebTechTestTaskDbContext : DbContext
    {
        public VebTechTestTaskDbContext(DbContextOptions<VebTechTestTaskDbContext> options) : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserLink> UserLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role { Id = 1, Name = "User" },
                    new Role { Id = 2, Name = "Admin" },
                    new Role { Id = 3, Name = "Support" },
                    new Role { Id = 4, Name = "SuperAdmin" },
                });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}