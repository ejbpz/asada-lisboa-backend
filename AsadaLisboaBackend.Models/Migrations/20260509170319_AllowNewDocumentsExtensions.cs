using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class AllowNewDocumentsExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Extension", "MimeType", "Name" },
                values: new object[,]
                {
                    { new Guid("1fa5211f-f4ea-42d1-8c5e-dee71d9a75a7"), ".doc", "application/msword", "Word" },
                    { new Guid("3f7294fc-4d1c-4a58-8576-9e2d404b2846"), ".xls", "application/vnd.ms-excel", "Excel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("1fa5211f-f4ea-42d1-8c5e-dee71d9a75a7"));

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: new Guid("3f7294fc-4d1c-4a58-8576-9e2d404b2846"));
        }
    }
}
