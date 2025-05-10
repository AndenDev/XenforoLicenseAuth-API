using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConfigurations
{
    public class UserAuthenticateConfiguration : IEntityTypeConfiguration<UserAuthenticate>
    {
        public void Configure(EntityTypeBuilder<UserAuthenticate> builder)
        {
            builder.ToTable("xf_user_authenticate");

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.SchemeClass).HasColumnName("scheme_class");
            builder.Property(e => e.Data).HasColumnName("data");

            builder.HasOne(e => e.User)
                   .WithOne(u => u.Authenticate)
                   .HasForeignKey<UserAuthenticate>(e => e.UserId);
        }
    }
}
