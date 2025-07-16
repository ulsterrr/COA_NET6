using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate_Migration_SqlServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmedCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("43db034a-98cc-42ee-8fff-c57016484f4d"), "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "EmailConfirmationCode", "EmailConfirmed", "EmailConfirmedCode", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "ResetPasswordCode", "UserName" },
                values: new object[] { new Guid("6e5d8fa8-fa96-419f-9c07-3e05b96b087e"), "defaultadmin@gmail.com", null, true, null, "Default", "Admin", new byte[] { 236, 216, 90, 190, 97, 167, 42, 227, 36, 101, 16, 219, 9, 62, 55, 237, 19, 168, 141, 230, 174, 74, 192, 59, 10, 221, 17, 201, 127, 234, 214, 186, 139, 34, 79, 200, 18, 124, 252, 145, 181, 174, 218, 210, 234, 12, 43, 27, 105, 236, 104, 144, 154, 196, 182, 195, 5, 32, 207, 7, 246, 211, 136, 75 }, new byte[] { 39, 94, 53, 253, 82, 216, 94, 220, 203, 172, 192, 184, 170, 203, 232, 20, 201, 234, 147, 90, 147, 51, 81, 25, 95, 110, 121, 191, 176, 197, 83, 192, 159, 170, 207, 125, 187, 154, 209, 216, 110, 50, 6, 137, 90, 173, 54, 192, 15, 193, 27, 179, 110, 192, 57, 124, 25, 12, 222, 115, 49, 33, 168, 70, 161, 163, 72, 88, 136, 225, 235, 182, 127, 230, 90, 211, 245, 91, 131, 184, 94, 199, 152, 74, 176, 115, 160, 122, 12, 137, 59, 142, 186, 58, 189, 223, 134, 3, 47, 200, 11, 200, 226, 217, 35, 118, 104, 109, 226, 207, 56, 91, 171, 4, 53, 123, 2, 155, 212, 184, 185, 58, 197, 126, 217, 65, 188, 52 }, null, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("43db034a-98cc-42ee-8fff-c57016484f4d"), new Guid("6e5d8fa8-fa96-419f-9c07-3e05b96b087e") });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
