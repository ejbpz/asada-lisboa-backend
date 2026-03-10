using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsadaLisboaBackend.Models.Migrations
{
    /// <inheritdoc />
    public partial class FluentApiConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDocument_Categories_CategoriesId",
                table: "CategoryDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDocument_Documents_DocumentsId",
                table: "CategoryDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImage_Categories_CategoriesId",
                table: "CategoryImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImage_Images_ImagesId",
                table: "CategoryImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryNew_Categories_CategoriesId",
                table: "CategoryNew");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryNew_News_NewsId",
                table: "CategoryNew");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Statuses_StatusId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Statuses_StatusId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Statuses_StatusId",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryNew",
                table: "CategoryNew");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryImage",
                table: "CategoryImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDocument",
                table: "CategoryDocument");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameTable(
                name: "CategoryNew",
                newName: "NewsCategories");

            migrationBuilder.RenameTable(
                name: "CategoryImage",
                newName: "ImagesCategories");

            migrationBuilder.RenameTable(
                name: "CategoryDocument",
                newName: "DocumentsCategories");

            migrationBuilder.RenameIndex(
                name: "IX_Images_StatusId",
                table: "Image",
                newName: "IX_Image_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_Slug",
                table: "Image",
                newName: "IX_Image_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryNew_NewsId",
                table: "NewsCategories",
                newName: "IX_NewsCategories_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImage_ImagesId",
                table: "ImagesCategories",
                newName: "IX_ImagesCategories_ImagesId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDocument_DocumentsId",
                table: "DocumentsCategories",
                newName: "IX_DocumentsCategories_DocumentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsCategories",
                table: "NewsCategories",
                columns: new[] { "CategoriesId", "NewsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImagesCategories",
                table: "ImagesCategories",
                columns: new[] { "CategoriesId", "ImagesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentsCategories",
                table: "DocumentsCategories",
                columns: new[] { "CategoriesId", "DocumentsId" });

            migrationBuilder.CreateIndex(
                name: "IX_VisualSettings_Order",
                table: "VisualSettings",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_News_PublicationDate",
                table: "News",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Extension",
                table: "DocumentTypes",
                column: "Extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PublicationDate",
                table: "Documents",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Order",
                table: "Contacts",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Charges_Name",
                table: "Charges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSections_Order",
                table: "AboutUsSections",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Status_Name",
                table: "Status",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_PublicationDate",
                table: "Image",
                column: "PublicationDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Status_StatusId",
                table: "Documents",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentsCategories_Categories_CategoriesId",
                table: "DocumentsCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentsCategories_Documents_DocumentsId",
                table: "DocumentsCategories",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Status_StatusId",
                table: "Image",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesCategories_Categories_CategoriesId",
                table: "ImagesCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCategories_Categories_CategoriesId",
                table: "NewsCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCategories_News_NewsId",
                table: "NewsCategories",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Status_StatusId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentsCategories_Categories_CategoriesId",
                table: "DocumentsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentsCategories_Documents_DocumentsId",
                table: "DocumentsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Status_StatusId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesCategories_Categories_CategoriesId",
                table: "ImagesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesCategories_Image_ImagesId",
                table: "ImagesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Status_StatusId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCategories_Categories_CategoriesId",
                table: "NewsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCategories_News_NewsId",
                table: "NewsCategories");

            migrationBuilder.DropIndex(
                name: "IX_VisualSettings_Order",
                table: "VisualSettings");

            migrationBuilder.DropIndex(
                name: "IX_News_PublicationDate",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_Extension",
                table: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PublicationDate",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Order",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Charges_Name",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AboutUsSections_Order",
                table: "AboutUsSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Status_Name",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsCategories",
                table: "NewsCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImagesCategories",
                table: "ImagesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_PublicationDate",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentsCategories",
                table: "DocumentsCategories");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "NewsCategories",
                newName: "CategoryNew");

            migrationBuilder.RenameTable(
                name: "ImagesCategories",
                newName: "CategoryImage");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.RenameTable(
                name: "DocumentsCategories",
                newName: "CategoryDocument");

            migrationBuilder.RenameIndex(
                name: "IX_NewsCategories_NewsId",
                table: "CategoryNew",
                newName: "IX_CategoryNew_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_ImagesCategories_ImagesId",
                table: "CategoryImage",
                newName: "IX_CategoryImage_ImagesId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_StatusId",
                table: "Images",
                newName: "IX_Images_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_Slug",
                table: "Images",
                newName: "IX_Images_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentsCategories_DocumentsId",
                table: "CategoryDocument",
                newName: "IX_CategoryDocument_DocumentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryNew",
                table: "CategoryNew",
                columns: new[] { "CategoriesId", "NewsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryImage",
                table: "CategoryImage",
                columns: new[] { "CategoriesId", "ImagesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDocument",
                table: "CategoryDocument",
                columns: new[] { "CategoriesId", "DocumentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDocument_Categories_CategoriesId",
                table: "CategoryDocument",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDocument_Documents_DocumentsId",
                table: "CategoryDocument",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImage_Categories_CategoriesId",
                table: "CategoryImage",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImage_Images_ImagesId",
                table: "CategoryImage",
                column: "ImagesId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryNew_Categories_CategoriesId",
                table: "CategoryNew",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryNew_News_NewsId",
                table: "CategoryNew",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Statuses_StatusId",
                table: "Documents",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Statuses_StatusId",
                table: "Images",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Statuses_StatusId",
                table: "News",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
