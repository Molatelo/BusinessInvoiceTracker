using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Data_Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "UsersLogin",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "text",
                nullable: true,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ClientTypes",
                type: "text",
                nullable: true,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "UsersLogin");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ClientTypes",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                oldStored: true);
        }
    }
}
