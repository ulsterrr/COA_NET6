using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class UserDepartmentConfiguration : IEntityTypeConfiguration<UserDepartment>
    {
        public void Configure(EntityTypeBuilder<UserDepartment> builder)
        {
            builder.HasKey(ud => new { ud.UserId, ud.DepartmentId });

            builder.HasOne(ud => ud.User)
                   .WithMany(u => u.UserDepartments)
                   .HasForeignKey(ud => ud.UserId);

            builder.HasOne(ud => ud.Department)
                   .WithMany(d => d.UserDepartments)
                   .HasForeignKey(ud => ud.DepartmentId);
        }
    }
}
