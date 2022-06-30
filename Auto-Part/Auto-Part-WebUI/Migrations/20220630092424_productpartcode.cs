using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class productpartcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPartCodes_Products_ProductId",
                table: "ProductPartCodes");

            migrationBuilder.DropIndex(
                name: "IX_ProductPartCodes_ProductId",
                table: "ProductPartCodes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductPartCodes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductPartCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPartCodes_ProductId",
                table: "ProductPartCodes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPartCodes_Products_ProductId",
                table: "ProductPartCodes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
