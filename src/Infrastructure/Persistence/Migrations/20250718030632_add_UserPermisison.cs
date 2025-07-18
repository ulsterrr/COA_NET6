using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class add_UserPermisison : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 227, 47, 3, 129, 20, 8, 165, 238, 57, 103, 248, 214, 80, 113, 21, 131, 81, 33, 203, 179, 82, 197, 230, 60, 48, 124, 19, 238, 225, 94, 119, 6, 88, 216, 135, 4, 96, 66, 128, 242, 245, 70, 169, 216, 195, 162, 238, 76, 189, 210, 25, 255, 86, 194, 248, 243, 250, 121, 145, 1, 119, 22, 31, 25 }, new byte[] { 189, 36, 73, 142, 226, 118, 100, 171, 80, 8, 130, 78, 212, 39, 135, 68, 190, 59, 92, 238, 76, 50, 145, 151, 166, 181, 113, 161, 235, 40, 39, 114, 149, 229, 70, 10, 29, 170, 15, 212, 237, 47, 115, 215, 140, 31, 48, 197, 87, 33, 194, 79, 248, 23, 223, 180, 172, 109, 67, 131, 222, 200, 243, 65, 172, 210, 136, 36, 128, 210, 55, 202, 109, 51, 108, 136, 49, 194, 138, 189, 45, 144, 161, 246, 25, 161, 98, 28, 242, 201, 0, 193, 157, 30, 6, 63, 236, 185, 240, 1, 229, 165, 139, 135, 105, 28, 52, 241, 189, 21, 11, 19, 66, 140, 59, 97, 124, 226, 106, 16, 173, 219, 15, 66, 219, 207, 248, 84 } });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 92, 86, 203, 80, 215, 154, 73, 106, 142, 87, 217, 187, 246, 218, 83, 71, 228, 252, 159, 187, 153, 77, 103, 145, 87, 63, 160, 81, 123, 228, 7, 221, 116, 13, 84, 104, 138, 20, 11, 15, 35, 86, 85, 0, 237, 140, 81, 255, 239, 97, 155, 19, 123, 163, 87, 248, 83, 61, 115, 152, 228, 228, 118, 105 }, new byte[] { 164, 94, 227, 146, 171, 139, 128, 128, 255, 95, 86, 211, 19, 12, 59, 199, 173, 225, 15, 46, 117, 75, 84, 67, 210, 170, 13, 137, 86, 21, 180, 114, 161, 84, 224, 117, 24, 80, 51, 224, 9, 41, 255, 107, 77, 138, 165, 87, 52, 115, 203, 93, 174, 199, 243, 156, 166, 17, 60, 93, 164, 113, 20, 118, 57, 195, 0, 107, 225, 206, 218, 109, 210, 129, 185, 14, 4, 97, 74, 8, 84, 150, 222, 81, 36, 26, 184, 209, 120, 168, 71, 48, 154, 226, 207, 64, 223, 231, 196, 216, 153, 184, 219, 131, 47, 119, 105, 158, 196, 100, 99, 89, 31, 194, 93, 72, 172, 122, 38, 15, 55, 105, 231, 158, 12, 73, 167, 66 } });
        }
    }
}
