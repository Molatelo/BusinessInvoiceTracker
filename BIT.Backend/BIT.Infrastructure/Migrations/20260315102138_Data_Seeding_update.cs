using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Data_Seeding_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "UsersLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "UsersLogin",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
