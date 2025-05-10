using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("xf_user");

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.Username).HasColumnName("username");
            builder.Property(e => e.UserGroupId).HasColumnName("user_group_id");
            builder.Property(e => e.Email).HasColumnName("email");

            builder.HasOne(e => e.UserGroup)
                   .WithMany()
                   .HasForeignKey(e => e.UserGroupId);

            builder.HasOne(e => e.Authenticate)
                   .WithOne(a => a.User)
                   .HasForeignKey<UserAuthenticate>(a => a.UserId);
        }
    }
}
