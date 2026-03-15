using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BIT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersLogin",
                table: "UsersLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersLogin",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UsersLogin",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UsersLogin",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UsersLogin_UserId",
                table: "UsersLogin",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersLogin",
                table: "UsersLogin",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UsersLogin_UserId",
                table: "UsersLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersLogin",
                table: "UsersLogin");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRoles_UserId_RoleId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersLogin");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UsersLogin");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UsersLogin");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersLogin",
                table: "UsersLogin",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });
        }
    }
}
