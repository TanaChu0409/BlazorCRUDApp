using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCRUDApp.Api.DatabaseMigrations
{
    /// <inheritdoc />
    public partial class UpdateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryGuid",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryGuid",
                table: "Product");
        }
    }
}