using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class productFinally : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PartCodes_PartCodeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PartCodeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PartCodeId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PartCodeName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartCodeName",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "PartCodeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PartCodeId",
                table: "Products",
                column: "PartCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PartCodes_PartCodeId",
                table: "Products",
                column: "PartCodeId",
                principalTable: "PartCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
