using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Computed_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "InvoiceItems",
                type: "numeric(18,2)",
                nullable: false,
                computedColumnSql: "\"Quantity\" * \"UnitPrice\"",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ClientTypes",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Roles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "InvoiceItems",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldComputedColumnSql: "\"Quantity\" * \"UnitPrice\"");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ClientTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "UPPER(REPLACE(\"Name\", ' ', '_'))");
        }
    }
}
