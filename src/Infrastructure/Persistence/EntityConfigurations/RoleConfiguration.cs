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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            const string AdminRoleId = "1";
            builder.Property(r => r.Name).IsRequired().HasMaxLength(16);
            builder.HasData(
                new Role
                {
                    Id = int.Parse(AdminRoleId),
                    Name = "Admin"
                }
            );
        }
    }
}
