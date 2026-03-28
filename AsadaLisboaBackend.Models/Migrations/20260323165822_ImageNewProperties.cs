using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class ImageNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Status_StatusId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Status_StatusId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesCategories_Image_ImagesId",
                table: "ImagesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Status_StatusId",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "Image",
                schema: "dbo",
                newName: "Images",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Status_Name",
                table: "Statuses",
                newName: "IX_Statuses_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Image_StatusId",
                table: "Images",
                newName: "IX_Images_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_Slug",
                table: "Images",
                newName: "IX_Images_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Image_PublicationDate",
                table: "Images",
                newName: "IX_Images_PublicationDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Statuses_StatusId",
                table: "Documents",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Statuses_StatusId",
                table: "Images",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesCategories_Images_ImagesId",
                table: "ImagesCategories",
                column: "ImagesId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Statuses_StatusId",
                table: "News",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Statuses_StatusId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Statuses_StatusId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesCategories_Images_ImagesId",
                table: "ImagesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Statuses_StatusId",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Statuses_Name",
                table: "Status",
                newName: "IX_Status_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Images_StatusId",
                table: "Image",
                newName: "IX_Image_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_Slug",
                table: "Image",
                newName: "IX_Image_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Images_PublicationDate",
                table: "Image",
                newName: "IX_Image_PublicationDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Status_StatusId",
                table: "Documents",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Status_StatusId",
                table: "Image",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesCategories_Image_ImagesId",
                table: "ImagesCategories",
                column: "ImagesId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Status_StatusId",
                table: "News",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
