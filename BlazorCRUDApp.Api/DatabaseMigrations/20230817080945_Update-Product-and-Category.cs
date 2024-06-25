using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCRUDApp.Api.DatabaseMigrations
{
    /// <inheritdoc />
    public partial class UpdateProductandCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Category_Uuid",
                table: "Category",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryGuid",
                table: "Product",
                column: "CategoryGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryGuid",
                table: "Product",
                column: "CategoryGuid",
                principalTable: "Category",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryGuid",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryGuid",
                table: "Product");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Category_Uuid",
                table: "Category");
        }
    }
}