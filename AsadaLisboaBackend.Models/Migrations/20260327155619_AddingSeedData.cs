using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddingSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Charges",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("219d007a-7678-4d0e-87d1-ecfa4ef0402a"), "Fiscal" },
                    { new Guid("2797641b-3f01-4fc7-bc88-96a4fbf20db7"), "Presidente" },
                    { new Guid("ba3b8826-24b2-44d8-8e1e-7af7202c125d"), "Vicepresidente" },
                    { new Guid("c6ea3e4f-ed3f-4cad-9697-bff213b8e5dd"), "Tesorero" },
                    { new Guid("cbec9bae-c444-4234-a851-1a2d9f544192"), "Secretario" },
                    { new Guid("cc1ad219-1f4b-47bc-a3d6-8723e8250375"), "Vocal 3" },
                    { new Guid("e0bfd8c2-0151-4de8-99b3-ac2b99800eb9"), "Vocal 2" },
                    { new Guid("f10c568a-e590-4402-88ac-8467cb6d8a9b"), "Vocal 1" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("171b840d-48b0-4f83-81dd-1921215898ac"), "Draft" },
                    { new Guid("7c3d1291-f268-4938-872e-d3f62ca8ab68"), "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("219d007a-7678-4d0e-87d1-ecfa4ef0402a"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("2797641b-3f01-4fc7-bc88-96a4fbf20db7"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("ba3b8826-24b2-44d8-8e1e-7af7202c125d"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("c6ea3e4f-ed3f-4cad-9697-bff213b8e5dd"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("cbec9bae-c444-4234-a851-1a2d9f544192"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("cc1ad219-1f4b-47bc-a3d6-8723e8250375"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("e0bfd8c2-0151-4de8-99b3-ac2b99800eb9"));

            migrationBuilder.DeleteData(
                table: "Charges",
                keyColumn: "Id",
                keyValue: new Guid("f10c568a-e590-4402-88ac-8467cb6d8a9b"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("171b840d-48b0-4f83-81dd-1921215898ac"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("7c3d1291-f268-4938-872e-d3f62ca8ab68"));
        }
    }
}
