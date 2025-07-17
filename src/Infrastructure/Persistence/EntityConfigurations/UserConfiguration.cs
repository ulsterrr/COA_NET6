using Application.Helpers;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            const string DefaultAdminUserId = "1";

            builder.Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.Email)
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.PasswordSalt)
                .IsRequired();

            var (passwordHash, passwordSalt) = PasswordHelper.CreateHash("123qwe");

            builder.HasData(
                new User
                {
                    Id = int.Parse(DefaultAdminUserId),
                    UserName = "admin",
                    FirstName = "Default",
                    LastName = "Admin",
                    PasswordHash = passwordHash,
                    EmailConfirmed=true,
                    PasswordSalt = passwordSalt,
                    Email = "defaultadmin@gmail.com"
                });
        }
    }
}