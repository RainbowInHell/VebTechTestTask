namespace VebTechTestTask.DAL.Configurations
{
    using Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "dbo");

            builder.Property(x => x.Name)
                .HasMaxLength(255);

            builder.Property(x => x.Email)
                .HasMaxLength(255);

            builder.Property(x => x.Password)
                .HasMaxLength(255);
        }
    }
}