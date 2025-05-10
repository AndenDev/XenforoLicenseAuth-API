using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.EntityConfigurations
{
    public class SessionActivityConfiguration : IEntityTypeConfiguration<SessionActivity>
    {
        public void Configure(EntityTypeBuilder<SessionActivity> builder)
        {
            builder.ToTable("xf_session_activity");

            builder.HasKey(e => new { e.UserId, e.UniqueKey });

            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.UniqueKey).HasColumnName("unique_key");
            builder.Property(e => e.Ip).HasColumnName("ip");
            builder.Property(e => e.ControllerName).HasColumnName("controller_name");
            builder.Property(e => e.ControllerAction).HasColumnName("controller_action");
            builder.Property(e => e.ViewState).HasColumnName("view_state");
            builder.Property(e => e.Params).HasColumnName("params");
            builder.Property(e => e.ViewDate).HasColumnName("view_date");
            builder.Property(e => e.RobotKey).HasColumnName("robot_key");
        }
    }
}
