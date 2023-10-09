namespace VebTechTestTask.DAL.Configurations
{
    using DAL.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserLinkConfiguration : IEntityTypeConfiguration<UserLink>
    {
        public void Configure(EntityTypeBuilder<UserLink> builder)
        {
            builder.ToTable("UserLink", "dbo");

            builder.HasOne(x => x.Role).WithMany(x => x.UserLinks).HasForeignKey(x => x.RoleId);
            builder.HasOne(x => x.User).WithMany(x => x.UserLinks).HasForeignKey(x => x.UserId);
        }
    }
}