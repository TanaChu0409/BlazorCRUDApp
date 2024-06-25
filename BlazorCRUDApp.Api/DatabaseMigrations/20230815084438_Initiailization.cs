using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCRUDApp.Api.DatabaseMigrations
{
    /// <inheritdoc />
    public partial class Initiailization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amonut = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}