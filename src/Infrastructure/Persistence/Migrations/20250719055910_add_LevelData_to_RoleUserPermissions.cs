using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class add_LevelData_to_RoleUserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LevelData",
                table: "UserPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LevelData",
                table: "RolePermissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 187, 94, 171, 128, 5, 107, 114, 32, 184, 24, 253, 98, 119, 25, 61, 107, 120, 164, 78, 50, 49, 13, 122, 164, 195, 127, 25, 107, 167, 170, 16, 60, 47, 73, 8, 178, 45, 56, 174, 22, 217, 67, 87, 178, 135, 165, 204, 46, 59, 165, 230, 167, 178, 175, 176, 83, 93, 57, 118, 110, 95, 167, 177, 183 }, new byte[] { 225, 11, 194, 174, 118, 15, 229, 87, 66, 187, 65, 57, 244, 143, 142, 43, 201, 11, 26, 53, 33, 54, 64, 205, 79, 222, 77, 101, 250, 190, 27, 137, 98, 60, 236, 49, 152, 46, 222, 129, 192, 152, 250, 35, 128, 160, 227, 43, 110, 91, 249, 252, 143, 180, 9, 181, 71, 121, 216, 61, 0, 75, 153, 2, 37, 30, 145, 43, 61, 40, 223, 109, 47, 96, 120, 61, 158, 248, 39, 74, 61, 167, 16, 215, 130, 48, 1, 145, 204, 71, 45, 158, 205, 192, 134, 161, 244, 204, 118, 7, 252, 120, 55, 27, 156, 141, 254, 98, 65, 118, 10, 200, 16, 208, 73, 226, 255, 97, 70, 205, 233, 67, 13, 114, 28, 210, 66, 59 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelData",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "LevelData",
                table: "RolePermissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 227, 47, 3, 129, 20, 8, 165, 238, 57, 103, 248, 214, 80, 113, 21, 131, 81, 33, 203, 179, 82, 197, 230, 60, 48, 124, 19, 238, 225, 94, 119, 6, 88, 216, 135, 4, 96, 66, 128, 242, 245, 70, 169, 216, 195, 162, 238, 76, 189, 210, 25, 255, 86, 194, 248, 243, 250, 121, 145, 1, 119, 22, 31, 25 }, new byte[] { 189, 36, 73, 142, 226, 118, 100, 171, 80, 8, 130, 78, 212, 39, 135, 68, 190, 59, 92, 238, 76, 50, 145, 151, 166, 181, 113, 161, 235, 40, 39, 114, 149, 229, 70, 10, 29, 170, 15, 212, 237, 47, 115, 215, 140, 31, 48, 197, 87, 33, 194, 79, 248, 23, 223, 180, 172, 109, 67, 131, 222, 200, 243, 65, 172, 210, 136, 36, 128, 210, 55, 202, 109, 51, 108, 136, 49, 194, 138, 189, 45, 144, 161, 246, 25, 161, 98, 28, 242, 201, 0, 193, 157, 30, 6, 63, 236, 185, 240, 1, 229, 165, 139, 135, 105, 28, 52, 241, 189, 21, 11, 19, 66, 140, 59, 97, 124, 226, 106, 16, 173, 219, 15, 66, 219, 207, 248, 84 } });
        }
    }
}
