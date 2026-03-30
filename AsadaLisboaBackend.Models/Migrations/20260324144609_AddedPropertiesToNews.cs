using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "News");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "News");
        }
    }
}
