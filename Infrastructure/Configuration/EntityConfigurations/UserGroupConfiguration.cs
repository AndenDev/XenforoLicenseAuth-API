using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConfigurations
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("xf_user_group");

            builder.HasKey(e => e.UserGroupId);
            builder.Property(e => e.UserGroupId).HasColumnName("user_group_id");
            builder.Property(e => e.Title).HasColumnName("title");
        }
    }
}
