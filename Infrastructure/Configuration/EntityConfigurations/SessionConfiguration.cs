using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConfigurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("xf_session");

            builder.HasKey(e => e.SessionId);
            builder.Property(e => e.SessionId).HasColumnName("session_id");
            builder.Property(e => e.SessionData).HasColumnName("session_data");
            builder.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
        }
    }
}
