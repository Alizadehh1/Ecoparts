using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class popularcarsvol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PopularCars_Brands_BrandId",
                table: "PopularCars");

            migrationBuilder.DropIndex(
                name: "IX_PopularCars_BrandId",
                table: "PopularCars");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "PopularCars");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PopularCars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductIds",
                table: "PopularCars",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PopularCars");

            migrationBuilder.DropColumn(
                name: "ProductIds",
                table: "PopularCars");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "PopularCars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PopularCars_BrandId",
                table: "PopularCars",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_PopularCars_Brands_BrandId",
                table: "PopularCars",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
