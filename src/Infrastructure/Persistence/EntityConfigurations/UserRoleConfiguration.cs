using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            const string AdminRoleId = "1";
            const string DefaultAdminUserId = "1";

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasData(
                new UserRole
                {
                    UserId = int.Parse(DefaultAdminUserId),
                    RoleId = int.Parse(AdminRoleId)
                });
        }
    }
}
