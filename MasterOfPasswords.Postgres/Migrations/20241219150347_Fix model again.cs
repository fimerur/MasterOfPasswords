using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterOfPasswords.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fixmodelagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Credentials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Credentials");
        }
    }
}
