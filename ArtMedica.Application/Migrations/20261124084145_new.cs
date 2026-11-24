using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtMedica.Application.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategories_CategoryId",
                table: "Services");

            migrationBuilder.CreateTable(
                name: "ServiceSubcategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ImgUrlId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSubcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSubcategories_Files_ImgUrlId",
                        column: x => x.ImgUrlId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceSubcategories_ServiceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSubcategories_CategoryId",
                table: "ServiceSubcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSubcategories_ImgUrlId",
                table: "ServiceSubcategories",
                column: "ImgUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceSubcategories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "ServiceSubcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceSubcategories_CategoryId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "ServiceSubcategories");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
