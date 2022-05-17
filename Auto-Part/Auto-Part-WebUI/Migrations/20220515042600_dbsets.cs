using Microsoft.EntityFrameworkCore.Migrations;

namespace Auto_Part_WebUI.Migrations
{
    public partial class dbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Brands_BrandId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPartCode_Product_ProductId",
                table: "ProductPartCode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricing_Product_ProductId",
                table: "ProductPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricing_ProductType_ProductTypeId",
                table: "ProductPricing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductType",
                table: "ProductType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPricing",
                table: "ProductPricing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPartCode",
                table: "ProductPartCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ProductType",
                newName: "ProductTypes");

            migrationBuilder.RenameTable(
                name: "ProductPricing",
                newName: "ProductPricings");

            migrationBuilder.RenameTable(
                name: "ProductPartCode",
                newName: "ProductPartCodes");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPricing_ProductTypeId",
                table: "ProductPricings",
                newName: "IX_ProductPricings_ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPartCode_ProductId",
                table: "ProductPartCodes",
                newName: "IX_ProductPartCodes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_BrandId",
                table: "Products",
                newName: "IX_Products_BrandId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypes",
                table: "ProductTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPricings",
                table: "ProductPricings",
                columns: new[] { "ProductId", "TypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPartCodes",
                table: "ProductPartCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPartCodes_Products_ProductId",
                table: "ProductPartCodes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricings_Products_ProductId",
                table: "ProductPricings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricings_ProductTypes_ProductTypeId",
                table: "ProductPricings",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPartCodes_Products_ProductId",
                table: "ProductPartCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricings_Products_ProductId",
                table: "ProductPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPricings_ProductTypes_ProductTypeId",
                table: "ProductPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypes",
                table: "ProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPricings",
                table: "ProductPricings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPartCodes",
                table: "ProductPartCodes");

            migrationBuilder.RenameTable(
                name: "ProductTypes",
                newName: "ProductType");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductPricings",
                newName: "ProductPricing");

            migrationBuilder.RenameTable(
                name: "ProductPartCodes",
                newName: "ProductPartCode");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandId",
                table: "Product",
                newName: "IX_Product_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPricings_ProductTypeId",
                table: "ProductPricing",
                newName: "IX_ProductPricing_ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPartCodes_ProductId",
                table: "ProductPartCode",
                newName: "IX_ProductPartCode_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductType",
                table: "ProductType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPricing",
                table: "ProductPricing",
                columns: new[] { "ProductId", "TypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPartCode",
                table: "ProductPartCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Brands_BrandId",
                table: "Product",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPartCode_Product_ProductId",
                table: "ProductPartCode",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricing_Product_ProductId",
                table: "ProductPricing",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPricing_ProductType_ProductTypeId",
                table: "ProductPricing",
                column: "ProductTypeId",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
