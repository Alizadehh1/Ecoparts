using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class productUPT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPartCodes");

            migrationBuilder.AddColumn<int>(
                name: "PartCodeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PartCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartCodes", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PartCodes_PartCodeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PartCodes");

            migrationBuilder.DropIndex(
                name: "IX_Products_PartCodeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PartCodeId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductPartCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPartCodes", x => x.Id);
                });
        }
    }
}
