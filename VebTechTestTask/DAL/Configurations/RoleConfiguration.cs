namespace VebTechTestTask.DAL.Configurations
{
    using Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role", "dbo");

            builder.Property(x => x.Name)
                .HasMaxLength(255);
        }
    }
}