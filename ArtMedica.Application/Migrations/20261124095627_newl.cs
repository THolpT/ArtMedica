using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtMedica.Application.Migrations
{
    /// <inheritdoc />
    public partial class newl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServiceSubcategories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServiceSubcategories");
        }
    }
}
